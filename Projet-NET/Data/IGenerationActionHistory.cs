﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.Data
{
    internal interface IGenerationActionHistory
    {
        #region Public Methods

        Action[,] CreateActionHistory(int lines, int cols);

        #endregion Public Methods
    }
}