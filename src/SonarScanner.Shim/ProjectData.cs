﻿/*
 * SonarQube Scanner for MSBuild
 * Copyright (C) 2016-2018 SonarSource SA
 * mailto:info AT sonarsource DOT com
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System;
using System.Collections.Generic;
using System.IO;
using SonarQube.Common;

namespace SonarScanner.Shim
{
    /// <summary>
    /// Contains the aggregated data from multiple ProjectInfos sharing the same GUID
    /// </summary>
    public class ProjectData
    {
        public ProjectData(ProjectInfo project)
        {
            Project = project;
        }

        public bool CoverageAnalysisExists(ILogger logger)
        {
            var visualStudioCoverageLocation = VisualStudioCoverageLocation;

            if (visualStudioCoverageLocation != null && !File.Exists(visualStudioCoverageLocation))
            {
                logger.LogWarning(Resources.WARN_CodeCoverageReportNotFound, visualStudioCoverageLocation);
                return false;
            }

            return true;
        }

        public string VisualStudioCoverageLocation => Project.TryGetAnalysisFileLocation(AnalysisType.VisualStudioCodeCoverage);

        public string Guid => Project.GetProjectGuidAsString();

        public ProjectInfoValidity Status { get; set; }

        public ProjectInfo Project { get; }

        /// <summary>
        /// All files referenced by the given project
        /// </summary>
        public ISet<string> ReferencedFiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// All files that belongs to this SonarQube module.
        /// </summary>
        public ICollection<string> SonarQubeModuleFiles { get; } = new List<string>();

        /// <summary>
        /// Roslyn analysis output files (json)
        /// </summary>
        public ICollection<string> RoslynReportFilePaths { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The folders where the protobuf files are generated
        /// </summary>
        public ICollection<string> AnalyzerOutPaths { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }
}
