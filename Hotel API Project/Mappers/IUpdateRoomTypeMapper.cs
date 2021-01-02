﻿using DataStructure;
using Hotel_API_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_API_Project.Mappers
{
    public interface IUpdateRoomTypeMapper
    {
        public RoomType MapUpdateRoomTypeViewModelToModel(UpdateRoomTypeViewModel updateRoomTypeViewModel, RoomType roomType);
    }
}
