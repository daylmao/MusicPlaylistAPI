﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.DTOs.Song
{
    public class SongUpdateDTO
    {
        public Guid UserId { get; set; }
        public string? Artist { get; set; }
        public string? Genre { get; set; }
        public float Duration { get; set; }
    }
}
