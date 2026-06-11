using GrupaA.DTOs;
using GrupaA.Exceptions;
using Microsoft.Data.SqlClient;

namespace GrupaA.Services;

public class ArtifactService(IConfiguration configuration) : IArtifactService
{
    public async Task AddArtifactWithProjectAsync(CreateProjectWithArtifactDto dto)
    {
        //UWAGA (!)
        // - czy instytucja istnieje?
        await using var connection = new SqlConnection(configuration.GetConnectionString("Default"));
        await using var command = new SqlCommand();
        
        await connection.OpenAsync();
        
        command.Connection = connection;
        
        
        //TRANSACTION -------------------------------------------------------------------
        await using var transaction = await connection.BeginTransactionAsync();
        command.Transaction = (SqlTransaction)transaction;
        try
        {
            // institution : existance 
            command.CommandText = """
                                  select 1 
                                  from institution
                                  where institutionId = @institutionId
                                  """;
            command.Parameters.AddWithValue("@InstitutionId", dto.Artifact.InstitutionId);
            var institutionExists = await command.ExecuteScalarAsync();

            if (institutionExists is null)
            {
                throw new NotFoundExcpetion($"Institution with id {dto.Artifact.InstitutionId} not found");
            }
            
            // artefact : insert
            command.CommandText = """
                                  insert into artifact (Name, OriginDate, InstitutionId)
                                  output inserted.ArtefactId
                                  values (@Name, @OriginDate, @InstitutionId)

                                  """;
            command.Parameters.AddWithValue("@Name", dto.Artifact.Name);
            command.Parameters.AddWithValue("@OriginDate", dto.Artifact.OriginDate);
            command.Parameters.AddWithValue("@InstitutionId", dto.Artifact.InstitutionId);
            
            var result = await command.ExecuteScalarAsync();
            int artifactId = Convert.ToInt32(result);
            command.Parameters.Clear();
        
            // project : insert 
            command.CommandText = """
                                  insert into preservation_project (ArtifactId, StartDate, EndDate, Objective)
                                  values (@ArtifactId, @StartDate, @EndDate, @Objective)
                                  """;
            command.Parameters.AddWithValue("@ArtifactId", artifactId);
            command.Parameters.AddWithValue("@StartDate", DateTime.Now);
            command.Parameters.AddWithValue("@EndDate", DBNull.Value);

            await command.ExecuteNonQueryAsync();
            command.Parameters.Clear();
            
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    
    }
}