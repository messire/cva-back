using CVA.Domain.Models;

namespace CVA.Tests.Unit.Domain.Models;

/// <summary>
/// Unit tests for the <see cref="DeveloperProfile"/> aggregate root.
/// </summary>
[Trait(Layer.Domain, Category.Models)]
public class DeveloperProfileTests
{
    /// <summary>
    /// Purpose: Verify that Create method correctly initializes a new developer profile.
    /// Should: Set all properties correctly and set creation/update timestamps.
    /// When: Valid arguments are provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void Create_Should_Initialize_DeveloperProfile(
        DeveloperId id, PersonName name, ContactInfo contact, SocialLinks social, VerificationStatus verification, OpenToWorkStatus openToWork)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        // Act
        var profile = DeveloperProfile.Create(id, name, null, null, null, contact, social, verification, openToWork, now);

        // Assert
        Assert.Equal(id, profile.Id);
        Assert.Equal(name, profile.Name);
        Assert.Equal(contact, profile.Contact);
        Assert.Equal(social, profile.Social);
        Assert.Equal(verification, profile.Verification);
        Assert.Equal(openToWork, profile.OpenToWork);
        Assert.Equal(now, profile.CreatedAt);
        Assert.Equal(now, profile.UpdatedAt);
        Assert.Empty(profile.Skills);
        Assert.Empty(profile.Projects);
        Assert.Empty(profile.WorkExperience);
    }

    /// <summary>
    /// Purpose: Verify that FromPersistence method correctly reconstructs a developer profile.
    /// Should: Restore all properties and collections.
    /// When: Valid state data is provided from persistence.
    /// </summary>
    [Theory, CvaAutoData]
    public void FromPersistence_Should_Reconstruct_DeveloperProfile(
        DeveloperId id, PersonName name, ContactInfo contact, SocialLinks social, VerificationStatus verification, OpenToWorkStatus openToWork,
        SkillTag[] skills, ProjectItem[] projects, WorkExperienceItem[] workExperience)
    {
        // Arrange
        var createdAt = DateTimeOffset.UtcNow.AddDays(-1);
        var updatedAt = DateTimeOffset.UtcNow;

        // Act
        var profile = DeveloperProfile.FromPersistence(
            id, name, null!, null!, null!, contact, social, verification, openToWork, skills, projects, workExperience, createdAt, updatedAt);

        // Assert
        Assert.Equal(id, profile.Id);
        Assert.Equal(name, profile.Name);
        Assert.Equal(skills.Length, profile.Skills.Count);
        Assert.Equal(projects.Length, profile.Projects.Count);
        Assert.Equal(workExperience.Length, profile.WorkExperience.Count);
        Assert.Equal(createdAt, profile.CreatedAt);
        Assert.Equal(updatedAt, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify adding a project to the profile.
    /// Should: Add project to collection and update the last modified timestamp.
    /// When: Valid project details are provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void AddProject_Should_Add_And_Touch(
        DeveloperProfile profile, ProjectName name, ProjectDescription desc, ProjectIcon icon, ProjectLink link, TechTag[] tech)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        // Act
        var projectId = profile.AddProject(name, desc, icon, link, tech, now);

        // Assert
        Assert.Contains(profile.Projects, item => item.Id == projectId);
        Assert.Equal(now, profile.UpdatedAt);
        
        var added = profile.Projects.First(item => item.Id == projectId);
        Assert.Equal(name, added.Name);
        Assert.Equal(desc, added.Description);
        Assert.Equal(tech.Length, added.TechStack.Count);
    }

    /// <summary>
    /// Purpose: Verify updating an existing project.
    /// Should: Update project properties and the profile's update timestamp.
    /// When: Valid updated details for an existing project are provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void UpdateProject_Should_Modify_Existing_And_Touch(
        DeveloperProfile profile, ProjectName newName, DateTimeOffset now)
    {
        // Arrange
        var existingId = profile.AddProject(ProjectName.From("Old"), null, null, ProjectLink.From("http://old.com"), [], now.AddMinutes(-1));

        // Act
        profile.UpdateProject(existingId, newName, null, null, ProjectLink.From("http://new.com"), [], now);

        // Assert
        var updated = profile.Projects.First(item => item.Id == existingId);
        Assert.Equal(newName, updated.Name);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify removing a project from the profile.
    /// Should: Remove project from collection and update the timestamp.
    /// When: Valid project ID is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void RemoveProject_Should_Remove_And_Touch(DeveloperProfile profile, DateTimeOffset now)
    {
        // Arrange
        var id = profile.AddProject(ProjectName.From("P"), null, null, ProjectLink.From("http://p.com"), [], now.AddMinutes(-1));

        // Act
        profile.RemoveProject(id, now);

        // Assert
        Assert.DoesNotContain(profile.Projects, item => item.Id == id);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify adding work experience to the profile.
    /// Should: Add work experience to collection and update the timestamp.
    /// When: Valid work experience details are provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void AddWorkExperience_Should_Add_And_Touch(
        DeveloperProfile profile, CompanyName company, RoleTitle role, DateRange period, TechTag[] tech, DateTimeOffset now)
    {
        // Act
        var id = profile.AddWorkExperience(company, null, role, null, period, tech, now);

        // Assert
        Assert.Contains(profile.WorkExperience, item => item.Id == id);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify removing work experience from the profile.
    /// Should: Remove the item and update the timestamp.
    /// When: Valid work experience ID is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void RemoveWorkExperience_Should_Remove_And_Touch(DeveloperProfile profile, DateTimeOffset now)
    {
        // Arrange
        var id = profile.AddWorkExperience(CompanyName.From("C"), null, RoleTitle.From("R"), null, DateRange.From(new DateOnly(2020,1,1), null), [], now.AddMinutes(-1));

        // Act
        profile.RemoveWorkExperience(id, now);

        // Assert
        Assert.DoesNotContain(profile.WorkExperience, item => item.Id == id);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify replacing all skills in the profile.
    /// Should: Replace existing skills with the new collection and update the timestamp.
    /// When: A collection of new skill tags is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void ReplaceSkills_Should_Replace_All_And_Touch(DeveloperProfile profile, SkillTag[] newSkills, DateTimeOffset now)
    {
        // Act
        profile.ReplaceSkills(newSkills, now);

        // Assert
        var expected = newSkills.Distinct().ToArray();
        Assert.Equal(expected.Length, profile.Skills.Count);
        foreach(var s in expected) Assert.Contains(s, profile.Skills);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify adding a skill to the profile.
    /// Should: Add skill to collection and update timestamp.
    /// When: A new skill tag is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void AddSkill_Should_Add_Unique_And_Touch(DeveloperProfile profile, SkillTag skill, DateTimeOffset now)
    {
        // Act
        profile.AddSkill(skill, now);

        // Assert
        Assert.Contains(skill, profile.Skills);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify that adding an existing skill does not create a duplicate.
    /// Should: Keep collection unchanged and not update the timestamp.
    /// When: An identical skill tag is added twice.
    /// </summary>
    [Theory, CvaAutoData]
    public void AddSkill_Should_Not_Add_Duplicate(DeveloperProfile profile, SkillTag skill, DateTimeOffset now)
    {
        // Arrange
        profile.AddSkill(skill, now.AddMinutes(-1));
        var lastUpdate = profile.UpdatedAt;

        // Act
        profile.AddSkill(skill, now);

        // Assert
        Assert.Single(profile.Skills, tag => tag.Equals(skill));
        Assert.Equal(lastUpdate, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify name change of the developer.
    /// Should: Update the Name property and the update timestamp.
    /// When: A new PersonName is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void ChangeName_Should_Update_And_Touch(DeveloperProfile profile, PersonName newName, DateTimeOffset now)
    {
        // Act
        profile.ChangeName(newName, now);

        // Assert
        Assert.Equal(newName, profile.Name);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify that RemoveProject returns false when the project does not exist.
    /// Should: Return false and not update the timestamp.
    /// When: Non-existent project ID is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void RemoveProject_Should_ReturnFalse_When_NotFound(DeveloperProfile profile, ProjectId nonExistentId, DateTimeOffset now)
    {
        // Act
        var result = profile.RemoveProject(nonExistentId, now);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Purpose: Verify that RemoveWorkExperience returns false when the work experience does not exist.
    /// Should: Return false and not update the timestamp.
    /// When: Non-existent work experience ID is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void RemoveWorkExperience_Should_ReturnFalse_When_NotFound(DeveloperProfile profile, WorkExperienceId nonExistentId, DateTimeOffset now)
    {
        // Act
        var result = profile.RemoveWorkExperience(nonExistentId, now);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Purpose: Verify years of experience calculation.
    /// Should: Correctly calculate full years of experience.
    /// When: Work experience entries are provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void GetYearsOfExperience_Should_CalculateDerivedValue(
        DeveloperId id, PersonName name, ContactInfo contact, SocialLinks social, VerificationStatus verification, OpenToWorkStatus openToWork)
    {
        // Arrange
        var now = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var profile = DeveloperProfile.Create(id, name, null, null, null, contact, social, verification, openToWork, now);
        
        var start = new DateOnly(2020, 1, 1);
        var end = new DateOnly(2023, 1, 1);
        
        profile.AddWorkExperience(
            CompanyName.From("C"), 
            null, 
            RoleTitle.From("R"), 
            null, 
            DateRange.From(start, end), 
            [], 
            now);

        // Act
        var years = profile.GetYearsOfExperience(now);

        // Assert
        Assert.Equal(3, years.Value);
    }

    /// <summary>
    /// Purpose: Verify years of experience calculation with ongoing work.
    /// Should: Use 'now' as the end date for ongoing experience.
    /// </summary>
    [Theory, CvaAutoData]
    public void GetYearsOfExperience_Should_UseNow_ForOngoingExperience(
        DeveloperId id, PersonName name, ContactInfo contact, SocialLinks social, VerificationStatus verification, OpenToWorkStatus openToWork)
    {
        // Arrange
        var now = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var profile = DeveloperProfile.Create(id, name, null, null, null, contact, social, verification, openToWork, now);
        
        var start = new DateOnly(2020, 1, 1);
        
        profile.AddWorkExperience(
            CompanyName.From("C"), 
            null, 
            RoleTitle.From("R"), 
            null, 
            DateRange.From(start, null), 
            [], 
            now);

        // Act
        var years = profile.GetYearsOfExperience(now);

        // Assert
        Assert.Equal(5, years.Value);
    }

    /// <summary>
    /// Purpose: Verify role change.
    /// Should: Update Role and update timestamp.
    /// When: A new RoleTitle is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void ChangeRole_Should_Update_And_Touch(DeveloperProfile profile, RoleTitle newRole, DateTimeOffset now)
    {
        // Act
        profile.ChangeRole(newRole, now);

        // Assert
        Assert.Equal(newRole, profile.Role);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify summary change.
    /// Should: Update Summary and update timestamp.
    /// When: A new ProfileSummary is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void ChangeSummary_Should_Update_And_Touch(DeveloperProfile profile, ProfileSummary newSummary, DateTimeOffset now)
    {
        // Act
        profile.ChangeSummary(newSummary, now);

        // Assert
        Assert.Equal(newSummary, profile.Summary);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify avatar change.
    /// Should: Update Avatar and update timestamp.
    /// When: A new Avatar is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void ChangeAvatar_Should_Update_And_Touch(DeveloperProfile profile, Avatar newAvatar, DateTimeOffset now)
    {
        // Act
        profile.ChangeAvatar(newAvatar, now);

        // Assert
        Assert.Equal(newAvatar, profile.Avatar);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify open to work status change.
    /// Should: Update OpenToWork and update timestamp.
    /// When: A new boolean value is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void SetOpenToWork_Should_Update_And_Touch(DeveloperProfile profile, bool isOpen, DateTimeOffset now)
    {
        // Act
        profile.SetOpenToWork(isOpen, now);

        // Assert
        Assert.Equal(isOpen, profile.OpenToWork.Value);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify contact information change.
    /// Should: Update Contact and update timestamp.
    /// When: A new ContactInfo is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void ChangeContact_Should_Update_And_Touch(DeveloperProfile profile, ContactInfo newContact, DateTimeOffset now)
    {
        // Act
        profile.ChangeContact(newContact, now);

        // Assert
        Assert.Equal(newContact, profile.Contact);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify social links change.
    /// Should: Update Social and update timestamp.
    /// When: A new SocialLinks is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void ChangeSocialLinks_Should_Update_And_Touch(DeveloperProfile profile, SocialLinks newSocial, DateTimeOffset now)
    {
        // Act
        profile.ChangeSocialLinks(newSocial, now);

        // Assert
        Assert.Equal(newSocial, profile.Social);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify verification status change.
    /// Should: Update Verification and update timestamp.
    /// When: A new VerificationStatus is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void SetVerified_Should_Update_And_Touch(DeveloperProfile profile, VerificationStatus newStatus, DateTimeOffset now)
    {
        // Act
        profile.SetVerified(newStatus, now);

        // Assert
        Assert.Equal(newStatus, profile.Verification);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify removing a skill.
    /// Should: Remove the skill and update the timestamp.
    /// When: An existing skill is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void RemoveSkill_Should_Remove_And_Touch(DeveloperProfile profile, SkillTag skill, DateTimeOffset now)
    {
        // Arrange
        profile.AddSkill(skill, now.AddMinutes(-1));

        // Act
        profile.RemoveSkill(skill, now);

        // Assert
        Assert.DoesNotContain(skill, profile.Skills);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify updating work experience.
    /// Should: Update the item and the profile's timestamp.
    /// When: Valid updated details for an existing work experience are provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void UpdateWorkExperience_Should_Modify_Existing_And_Touch(
        DeveloperProfile profile, CompanyName newCompany, DateTimeOffset now)
    {
        // Arrange
        var id = profile.AddWorkExperience(CompanyName.From("Old"), null, RoleTitle.From("R"), null, DateRange.From(new DateOnly(2020,1,1), null), [], now.AddMinutes(-1));

        // Act
        profile.UpdateWorkExperience(id, newCompany, null, RoleTitle.From("New"), null, DateRange.From(new DateOnly(2020,1,1), null), [], now);

        // Assert
        var updated = profile.WorkExperience.First(item => item.Id == id);
        Assert.Equal(newCompany, updated.Company);
        Assert.Equal(now, profile.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify handling of non-existent project during update.
    /// Should: Throw InvalidOperationException.
    /// When: A random ProjectId is provided to UpdateProject.
    /// </summary>
    [Theory, CvaAutoData]
    public void UpdateProject_Should_Throw_When_NotFound(
        DeveloperProfile profile, ProjectId nonExistentId, ProjectName name, ProjectLink link, DateTimeOffset now)
    {
        // Act & Assert
        Assert.Throws<ProjectNotFoundException>(() => 
            profile.UpdateProject(nonExistentId, name, null, null, link, [], now));
    }

    /// <summary>
    /// Purpose: Verify handling of non-existent work experience during update.
    /// Should: Throw WorkExperienceNotFoundException.
    /// When: A random WorkExperienceId is provided to UpdateWorkExperience.
    /// </summary>
    [Theory, CvaAutoData]
    public void UpdateWorkExperience_Should_Throw_When_NotFound(
        DeveloperProfile profile, WorkExperienceId nonExistentId, CompanyName company, RoleTitle role, DateRange period, DateTimeOffset now)
    {
        // Act & Assert
        Assert.Throws<WorkExperienceNotFoundException>(() => 
            profile.UpdateWorkExperience(nonExistentId, company, null, role, null, period, [], now));
    }
}
