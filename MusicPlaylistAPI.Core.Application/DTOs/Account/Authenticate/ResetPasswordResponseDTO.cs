﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.DTOs.Account.Authenticate
{
    public class ResetPasswordResponseDTO
    {
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
