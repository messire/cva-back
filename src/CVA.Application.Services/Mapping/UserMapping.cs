namespace CVA.Application.Services;

/// <summary>
/// Provides mapping functionalities related to user entities within the application.
/// </summary>
internal static class UserMapping
{
    extension(User model)
    {
        /// <summary>
        /// Maps a user entity to its corresponding data transfer object (DTO).
        /// </summary>
        /// <returns>
        /// A <see cref="UserDto"/> instance containing the mapped data from the user entity.
        /// </returns>
        public UserDto ToDto()
            => new(model.Name, model.Surname, model.Email)
            {
                Id = model.Id,
                Phone = model.Phone,
                Photo = model.Photo,
                Birthday = model.Birthday,
                SummaryInfo = model.SummaryInfo,
                Skills = model.Skills.ToArray(),
                WorkExperience = model.WorkExperience.ToDto(),
            };
    }

    extension(IEnumerable<User> models)
    {
        /// <summary>
        /// Converts a user entity into its data transfer object (DTO) representation.
        /// </summary>
        /// <returns>
        /// A <see cref="UserDto"/> containing the mapped data from the user entity.
        /// </returns>
        public IEnumerable<UserDto> ToDto()
            => models.Select(user => user.ToDto());
    }

    extension(UserDto dto)
    {
        /// <summary>
        /// Converts a <see cref="UserDto"/> instance to its corresponding <see cref="User"/> domain model.
        /// </summary>
        /// <returns>
        /// A <see cref="User"/> instance containing the mapped data from the <see cref="UserDto"/>.
        /// </returns>
        public User ToModel()
        {
            var user  = User.Create(dto.Name, dto.Surname, dto.Email);
            user.UpdateProfile(dto.Phone, dto.Birthday, dto.SummaryInfo);
            user.UpdatePhoto(dto.Photo);
            user.ReplaceSkills(dto.Skills);
            user.ReplaceWorkExperience(dto.WorkExperience?.ToModel());
            return user;
        }
    }
}