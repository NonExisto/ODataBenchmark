using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ODataBenchmark.DataModel;

namespace ODataBenchmark
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<BenchmarkContext>(opt => opt.EnableSensitiveDataLogging().UseInMemoryDatabase("BookLists"));
			services.AddControllers(opt => { opt.MaxIAsyncEnumerableBufferLimit = 150000; opt.OutputFormatters.Add(new GooseFormatter()); });
			services.AddOData(opt => opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(200).AddModel("odata", EdmModelBuilder.GetEdmModel()));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints => endpoints.MapControllers());
		}
	}
}
