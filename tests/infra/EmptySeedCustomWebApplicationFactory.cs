public class EmptySeedCustomWebApplicationFactory : CustomWebApplicationFactory
{
    protected override bool SeedDatabase => false;
}