namespace CVA.Application.Services;

/// <summary>
/// Provides utility methods and functionality for mapping work-related data.
/// </summary>
internal static class WorkMapping
{
    extension(Work model)
    {
        /// <summary>
        /// Converts a <see cref="Work"/> entity to a <see cref="WorkDto"/> instance.
        /// </summary>
        /// <returns>
        /// A <see cref="WorkDto"/> object containing information mapped from the <see cref="Work"/> entity.
        /// </returns>
        public WorkDto ToDto()
            => new()
            {
                CompanyName = model.CompanyName,
                Role = model.Role,
                Description = model.Description,
                Location = model.Location,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Achievements = model.Achievements.ToArray(),
                TechStack = model.TechStack.ToArray()
            };
    }

    extension(IEnumerable<Work> models)
    {
        /// <summary>
        /// Converts a collection of <see cref="Work"/> entities to an array of <see cref="WorkDto"/> instances.
        /// </summary>
        /// <returns>
        /// An array of <see cref="WorkDto"/> objects containing information mapped from the <see cref="Work"/> entities.
        /// </returns>
        public WorkDto[] ToDto()
            => models.Select(work => work.ToDto()).ToArray();
    }

    extension(WorkDto dto)
    {
        /// <summary>
        /// Converts a <see cref="WorkDto"/> instance to a <see cref="Work"/> entity.
        /// </summary>
        /// <returns>
        /// A <see cref="Work"/> object containing information mapped from the <see cref="WorkDto"/> instance.
        /// </returns>
        public Work ToModel()
            => Work.Create(
                dto.CompanyName,
                dto.Role,
                dto.StartDate,
                dto.EndDate,
                dto.Description,
                dto.Location,
                dto.Achievements,
                dto.TechStack);
    }

    extension(WorkDto[] dtos)
    {
        /// <summary>
        /// Converts a <see cref="WorkDto"/> instance to a <see cref="Work"/> entity.
        /// </summary>
        /// <returns>
        /// A <see cref="Work"/> object containing information mapped from the <see cref="WorkDto"/> instance.
        /// </returns>
        public List<Work> ToModel()
            => dtos.Select(dto => dto.ToModel()).ToList();
    }
}