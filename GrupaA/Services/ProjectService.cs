using GrupaA.DTOs;
using GrupaA.Exceptions;
using Microsoft.Data.SqlClient;

namespace GrupaA.Services;

public class ProjectService(IConfiguration configuration) : IProjectService
{
    public async Task<ProjectDto> GetProjectByIdAsync(int id)
    {
         ProjectDto? projectDto = null;
        Dictionary<int,StaffDto> staff = new Dictionary<int, StaffDto>();
        
        //Przekopiować
        await using var connection = new SqlConnection(configuration.GetConnectionString("Default"));
        await using var command = new SqlCommand();
        await connection.OpenAsync();
        command.Connection = connection;
        
        command.CommandText = """
                                select p.ProjectId, p.Objective, p.startDate, p.EndDate,
                                      a.Name, a.originDate, 
                                      i.InstitutionId, i.Name, i.FoundedYear,
                                      s.StaffId, s.FirstName, s.LastName, s.HireDate,
                                      sa.Role
                              from Preservation_Project as p
                              left join artifact as a on a.ArtifactId = p.ArtifactId
                              left join Staff_Assignment as sa on sa.ProjectId = p.ProjectId
                              left join Staff as s on s.StaffId = sa.StaffId
                              left join Institution as i on a.InstitutionId = i.InstitutionId
                              where p.ProjectId = @ProjectId
                              """;

        command.Parameters.AddWithValue("@ProjectId", id);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            projectDto ??= new ProjectDto
            {
                ProjectId = reader.GetInt32(0),
                Objective = reader.GetString(1),
                StartDate = reader.GetDateTime(2),
                EndDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                Artifact = new ArtifactDto
                {
                    Name  = reader.GetString(4),
                    OriginDate = reader.GetDateTime(5),
                    Institution = new InstitutionDto
                    {
                        InstitutionId = reader.GetInt32(6),
                        Name = reader.GetString(7),
                        FoundedYeard =  reader.GetInt32(8),
                    }
                },
                StaffAssignments = new List<StaffDto>()
            };
            
            //if(reader.IsDBNull(2)) continue;

            var staffId = reader.GetInt32(9);
            if (!staff.ContainsKey(staffId))
            {
                staff[staffId] = new StaffDto
                {
                   FirstName = reader.GetString(10),
                   LastName = reader.GetString(11),
                   HireDate = reader.GetDateTime(12),
                   Role = reader.GetString(13)
                };
            }
        }

        //END
        if (projectDto is null)
        {
            throw new NotFoundExcpetion($"Project with id {id} not found");
        }

        projectDto.StaffAssignments = staff.Values.ToList();
        return projectDto;
    }
}