﻿using Festispec.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IInspectionService
    {
        #region planned Event
        PlannedInspection GetPlannedInspection(int plannedInspectionId);
        Task<PlannedInspection> CreatePlannedInspection(Festival festival);
        Task<PlannedInspection> CreatePlannedInspection(Festival festival, Questionnaire questionnaire);
        Task RemoveInspection(int PlannedInspectionId);
        #endregion

    }
}
