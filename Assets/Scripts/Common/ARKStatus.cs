using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Common
{
    public enum ARKStatus : int
    {
        ARK_SUCCESS = 0,

        ARK_ERROR_GENERAL   = -500,
        ARK_INVALID_ARG     = -501,

        ARK_NOT_IMPLEMENTED = -800,
    }
}
