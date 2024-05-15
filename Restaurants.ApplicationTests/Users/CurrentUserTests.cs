using FluentAssertions;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Users.Tests;

public class CurrentUserTests
{
    // TestMethod_Scenario_ExpectedResult
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);
        
        // act
        var isInRole = currentUser.IsInRole(roleName);
        
        // assert
        isInRole.Should().BeTrue();
    }
    
    [Fact()]
    public void IsInRole_WithNoMatchingRole_ShouldReturnFalse()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);
        
        // act
        var isInRole = currentUser.IsInRole(UserRoles.Owner);
        
        // assert
        isInRole.Should().BeFalse();
    }
    
    [Fact()]
    public void IsInRole_WithNoMatchingRoleCase_ShouldReturnFalse()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);
        
        // act
        var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());
        
        // assert
        isInRole.Should().BeFalse();
    }
}