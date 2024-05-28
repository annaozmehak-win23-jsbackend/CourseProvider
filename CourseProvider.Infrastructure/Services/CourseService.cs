using CourseProvider.Infrastructure.Data.Contexts;
using CourseProvider.Infrastructure.Factories;
using CourseProvider.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProvider.Infrastructure.Services;

public interface ICourseService
{
    Task<Course> CreateCourseAsync(CourseCreateRequest request);
    Task<Course> GetCourseByIdAsync(string id);
    Task<IEnumerable<Course>> GetCoursesAsync();
    Task<Course> UpdateCourseAsync(CourseUpdateRequest request);
    Task<bool> DeleteCourseAsync(string id);

}
public class CourseService(IDbContextFactory<DataContext> contextFactory) : ICourseService
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;


    public async Task<Course> CreateCourseAsync(CourseCreateRequest request)
    {
        await using var context = _contextFactory.CreateDbContext();

        var courseEntity = CourseFactory.Create(request);
        context.Courses.Add(courseEntity);
        await context.SaveChangesAsync();

        return CourseFactory.Create(courseEntity);
    }

    public async Task<bool> DeleteCourseAsync(string id)
    {
        await using var context = _contextFactory.CreateDbContext();
        var courseEntity = await context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        if (courseEntity == null) return false;

        context.Courses.Remove(courseEntity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Course> GetCourseByIdAsync(string id)
    {
        await using var context = _contextFactory.CreateDbContext();
        var courseEntity = await context.Courses.FirstOrDefaultAsync(c => c.Id == id);

        return courseEntity == null ? null! : CourseFactory.Create(courseEntity);
    }

    public async Task<IEnumerable<Course>> GetCoursesAsync()
    {
        await using var context = _contextFactory.CreateDbContext();
        var courseEntities = await context.Courses.ToListAsync();

        return courseEntities.Select(CourseFactory.Create);
    }

    public async Task<Course> UpdateCourseAsync(CourseUpdateRequest request)
    {
        await using var context = _contextFactory.CreateDbContext();
        var existingCourse = await context.Courses
            .Include(c => c.Authors)
            .Include(c => c.Prices)
            .Include(c => c.Content)
            .ThenInclude(c => c.ProgramDetails)
            .FirstOrDefaultAsync(c => c.Id == request.Id);

        if (existingCourse == null) return null!;

        var updatedCourseEntity = CourseFactory.Create(request);
        updatedCourseEntity.Id = existingCourse.Id;

        context.Entry(existingCourse).CurrentValues.SetValues(updatedCourseEntity);
        
        existingCourse.Content!.Description = updatedCourseEntity.Content!.Description;
        existingCourse.Content.Includes = updatedCourseEntity.Content.Includes;
        
        existingCourse.Authors!.Clear();
        foreach (var author in updatedCourseEntity.Authors!)
        {
            existingCourse.Authors.Add(author);
        }

        existingCourse.Content!.ProgramDetails!.Clear();
        foreach (var programDetail in updatedCourseEntity.Content!.ProgramDetails!)
        {
            existingCourse.Content.ProgramDetails.Add(programDetail);
        }

        existingCourse.Prices = updatedCourseEntity.Prices;

        await context.SaveChangesAsync();
        return CourseFactory.Create(existingCourse);
    }
}
