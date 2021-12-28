/**
 * @license
 * Copyright 2011 The Emscripten Authors
 * SPDX-License-Identifier: MIT
 */

#include "mediapipe_api/gpu/gl_wrapper.h"

#if defined(__EMSCRIPTEN__)

EM_JS(GLint, __wrap_glGetUniformLocation, (GLuint program, const GLchar *name), {
  // Returns the index of '[' character in an uniform that represents an array of uniforms (e.g. colors[10])
  // Closure does counterproductive inlining: https://github.com/google/closure-compiler/issues/3203, so prevent
  // inlining manually.
  /** @noinline */
  function getLeftBracePos(name) { return name.slice(-1) == ']' && name.lastIndexOf('['); }

#if GL_ASSERTIONS
  GL.validateGLObjectID(GL.programs, program, 'glGetUniformLocation', 'program');
#endif
  name = UTF8ToString(name);

#if GL_ASSERTIONS
  assert(!name.includes(' '), 'Uniform names passed to glGetUniformLocation() should not contain spaces! (received "' + name + '")');
#endif

  if (program = GL.programs[program]) {
    var uniformLocsById = program.uniformLocsById;                  // Maps GLuint -> WebGLUniformLocation
    var uniformSizeAndIdsByName = program.uniformSizeAndIdsByName;  // Maps name -> [uniform array length, GLuint]
    var i, j;
    var arrayIndex = 0;
    var uniformBaseName = name;

    // Invariant: when populating integer IDs for uniform locations, we must maintain the precondition that
    // arrays reside in contiguous addresses, i.e. for a 'vec4 colors[10];', colors[4] must be at location colors[0]+4.
    // However, user might call glGetUniformLocation(program, "colors") for an array, so we cannot discover based on the user
    // input arguments whether the uniform we are dealing with is an array. The only way to discover which uniforms are arrays
    // is to enumerate over all the active uniforms in the program.
    var leftBrace = getLeftBracePos(name);

    // On the first time invocation of glGetUniformLocation on this shader program:
    // initialize cache data structures and discover which uniforms are arrays.
    if (!uniformLocsById) {
      // maps GLint integer locations to WebGLUniformLocations
      program.uniformLocsById = uniformLocsById = {};
      // maps integer locations back to uniform name strings, so that we can lazily fetch uniform array locations
      program.uniformArrayNamesById = {};

      for (i = 0; i < GLctx.getProgramParameter(program, 0x8B86 /*GL_ACTIVE_UNIFORMS*/); ++i) {
        var u = GLctx.getActiveUniform(program, i);
        var nm = u.name;
        var sz = u.size;
        var lb = getLeftBracePos(nm);
        var arrayName = lb > 0 ? nm.slice(0, lb) : nm;

        // Assign a new location.
        var id = program.uniformIdCounter;
        program.uniformIdCounter += sz;

        // Eagerly get the location of the uniformArray[0] base element.
        // The remaining indices >0 will be left for lazy evaluation to
        // improve performance. Those may never be needed to fetch, if the
        // application fills arrays always in full starting from the first
        // element of the array.
        uniformSizeAndIdsByName[arrayName] = [ sz, id ];

        // Store placeholder integers in place that highlight that these
        // >0 index locations are array indices pending population.
        for (j = 0; j < sz; ++j) {
          uniformLocsById[id] = j;
          program.uniformArrayNamesById[id++] = arrayName;
        }
      }
    }

    // If user passed an array accessor "[index]", parse the array index off the accessor.
    if (leftBrace > 0) {
#if GL_ASSERTIONS
      assert(name.slice(leftBrace + 1).length == 1 || !isNaN(jstoi_q(name.slice(leftBrace + 1))),
             'Malformed input parameter name "' + name + '" passed to glGetUniformLocation!');
#endif
      arrayIndex = jstoi_q(name.slice(leftBrace + 1)) >>>
                   0;  // "index]", coerce parseInt(']') with >>>0 to treat "foo[]" as "foo[0]" and foo[-1] as unsigned out-of-bounds.
      uniformBaseName = name.slice(0, leftBrace);
    }

    // Have we cached the location of this uniform before?
    var sizeAndId = uniformSizeAndIdsByName[uniformBaseName];  // A pair [array length, GLint of the uniform location]

    // If an uniform with this name exists, and if its index is within the array limits (if it's even an array),
    // query the WebGLlocation, or return an existing cached location.
    if (sizeAndId && arrayIndex < sizeAndId[0]) {
      arrayIndex += sizeAndId[1];  // Add the base location of the uniform to the array index offset.
      if ((uniformLocsById[arrayIndex] = uniformLocsById[arrayIndex] || GLctx.getUniformLocation(program, name))) {
        return arrayIndex;
      }
    }
  }
#if GL_TRACK_ERRORS
  else {
    // N.b. we are currently unable to distinguish between GL program IDs that never existed vs GL program IDs that have been deleted,
    // so report GL_INVALID_VALUE in both cases.
    GL.recordError(0x501 /* GL_INVALID_VALUE */);
  }
#endif
  return -1;
});

EM_JS(GLuint, __wrap_glCreateShader, (GLenum type), {
  var id = GL.getNewId(GL.shaders);
  var shader = GLctx.createShader(type);
  GL.shaders[id] = shader;

  return id;
});

EM_JS(void, __wrap_glShaderSource, (GLuint shader, GLsizei count, const GLchar *const *str, const GLint *length), {
#if GL_ASSERTIONS
  GL.validateGLObjectID(GL.shaders, shader, 'glShaderSource', 'shader');
#endif

  console.log("wrapped glShaderSource");
  var source = GL.getSource(shader, count, str, length);
  GLctx.shaderSource(GL.shaders[shader], source);
});

EM_JS(void, __wrap_glAttachShader, (GLuint program, GLuint shader), {
#if GL_ASSERTIONS
  GL.validateGLObjectID(GL.programs, program, 'glAttachShader', 'program');
  GL.validateGLObjectID(GL.shaders, shader, 'glAttachShader', 'shader');
#endif
  GLctx.attachShader(GL.programs[program], GL.shaders[shader]);
});

EM_JS(void, __wrap_glLinkProgram, (GLuint program), {
#if GL_ASSERTIONS
  GL.validateGLObjectID(GL.programs, program, 'glLinkProgram', 'program');
#endif
  program = GL.programs[program];
  GLctx.linkProgram(program);
#if GL_DEBUG
  var log = (GLctx.getProgramInfoLog(program) || '').trim();
  if (log) console.error('glLinkProgram: ' + log);
  if (program.uniformLocsById) console.log('glLinkProgram invalidated ' + Object.keys(program.uniformLocsById).length + ' uniform location mappings');
#endif
  // Invalidate earlier computed uniform->ID mappings, those have now become stale
  program.uniformLocsById = 0;  // Mark as null-like so that glGetUniformLocation() knows to populate this again.
  program.uniformSizeAndIdsByName = {};
});

EM_JS(void, __wrap_glUseProgram, (GLuint program), {
#if GL_ASSERTIONS
  GL.validateGLObjectID(GL.programs, program, 'glUseProgram', 'program');
#endif
  program = GL.programs[program];
  GLctx.useProgram(program);
  // Record the currently active program so that we can access the uniform
  // mapping table of that program.
  GLctx.currentProgram = program;
});

#else

__wrap_glGetUniformLocation = glGetUniformLocation;
__wrap_glCreateShader = glCreateShader;
__wrap_glShaderSource = glShaderSource;
__wrap_glAttachShader = glAttachShader;
__wrap_glLinkProgram = glLinkProgram;
__wrap_glUseProgram = glUseProgram;

#endif  // defined(__EMSCRIPTEN__)
