module.exports.readVersion = function (contents) {
  var pluginVersionIdPos = contents.indexOf("id: plugin_version");
  var matchedVersion = contents.substring(pluginVersionIdPos).match(/placeholder: v(.*)/);
  return matchedVersion[1];
};

module.exports.writeVersion = function (contents, version) {
  var pluginVersionIdPos = contents.indexOf("id: plugin_version");
  var matchedVersion = contents.substring(pluginVersionIdPos).match(/placeholder: v(.*)/);
  var versionStartPos = pluginVersionIdPos + matchedVersion.index;
  var versionEndPos = versionStartPos + matchedVersion[0].length;
  return contents.substring(0, versionStartPos) + `placeholder: v${version}` + contents.substring(versionEndPos);
};
