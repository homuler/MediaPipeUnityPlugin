# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

"""Utility Functions"""

def concat_dict(x, y):
    z = {}
    z.update(x)
    z.update(y)
    return z

def concat_dict_and_select(x, select_cmd):
    result = {}

    for key in select_cmd.keys():
        result[key] = concat_dict(x, select_cmd[key])

    return select(result)
