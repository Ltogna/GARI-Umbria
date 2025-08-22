using AgronicaDataProvider6;
using AgronicaDataProvider6.Interfaces;
using AgronicaDataProvider6.Models;
using AgronicaNetCore.Anagrafe.BIZ.Services.AlberoGerarchiaImprese;
using AgronicaNetCore.Anagrafe.BIZ.Services.Budget;
using AgronicaNetCore.Anagrafe.BIZ.Services.CodiciAnagrafe;
using AgronicaNetCore.Anagrafe.BIZ.Services.ConfrontoCatasto;
using AgronicaNetCore.Anagrafe.BIZ.Services.Impianti;
using AgronicaNetCore.Anagrafe.BIZ.Services.Imprese;
using AgronicaNetCore.Anagrafe.BIZ.Services.PianoColturale;
using AgronicaNetCore.Anagrafe.BIZ.Services.Zone;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.Budget;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.CodiciAnagrafe;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.Fabbricati;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.GerarchiaImprese;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.Impianti;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.Impresa;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.PianoColturale;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.Programmazione;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.Zone;
using AgronicaNetCore.Authentication.BIZ.Services.Authentication;
using AgronicaNetCore.Base.DataLayer.Security;
using AgronicaNetCore.Base.Services.Culture;
using AgronicaNetCore.Base.Services.Security;
using AgronicaNetCore.Base.Utility;
using AgronicaNetCore.DefaultPianiColturali.BIZ.Services;
using AgronicaNetCore.DomandaIrrigua.BIZ.Services;
using AgronicaNetCore.DomandaIrrigua.DAL.DataLayer;
using AgronicaNetCore.FiltroRicerca.BIZ.Services;
using AgronicaNetCore.FiltroRicerca.DAL.DataLayer.FiltroRicerca;
using AgronicaNetCore.Gis.BIZ.Services.Gis;
using AgronicaNetCore.Gis.DAL.DataLayer.Gis;
using AgronicaNetCore.MetaSchema.BIZ.Services.Contatti;
using AgronicaNetCore.MetaSchema.BIZ.Services.DivisioniAmministrative;
using AgronicaNetCore.MetaSchema.BIZ.Services.Farmaci;
using AgronicaNetCore.MetaSchema.BIZ.Services.GruppiOperazione;
using AgronicaNetCore.MetaSchema.BIZ.Services.LettureStatiche;
using AgronicaNetCore.MetaSchema.BIZ.Services.Operazioni;
using AgronicaNetCore.MetaSchema.BIZ.Services.ParcoMacchine;
using AgronicaNetCore.MetaSchema.BIZ.Services.Servizi;
using AgronicaNetCore.MetaSchema.BIZ.Services.SpecieVegetali;
using AgronicaNetCore.MetaSchema.BIZ.Services.UnitaMisuraConversione;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.Contatti;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.Ditte;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.DivisioniAmministrative;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.Farmaci;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.GruppiOperazione;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.LettureStatiche;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.MacchineCaratteristiche;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.Operazione;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.Operazioni;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.ParcoMacchine;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.PrincipiAttivi;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.Servizi;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.SpecieVegetali;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.UnitaMisuraConversione;
using AgronicaNetCore.Note.BIZ.Services;
using AgronicaNetCore.Note.DAL.DataLayer.NoteInterventoUtilizzo;
using AgronicaNetCore.Operazione.BIZ.Services.Agenda;
using AgronicaNetCore.Operazione.BIZ.Services.OperazioneCausale;
using AgronicaNetCore.Operazione.BIZ.Services.ReportImpiegoFitosanitari;
using AgronicaNetCore.Operazione.BIZ.Services.Trattamenti;
using AgronicaNetCore.Operazione.DAL.DataLayer.Agenda;
using AgronicaNetCore.Operazione.DAL.DataLayer.Agronica_Log_Agenda;
using AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Destinazioni;
using AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Dettaglio_Tecnico;
using AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Dettaglio_Tecnico_Extra;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti_Dettagli;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti_Zoo;
using AgronicaNetCore.Operazione.DAL.DataLayer.OperazioneCausale;
using AgronicaNetCore.Operazione.DAL.DataLayer.ReportImpiegoProdottiFitosanitari;
using AgronicaNetCore.Operazione.DAL.DataLayer.Trattamenti;
using AgronicaNetCore.Operazione.DAL.DataLayer.Ricette;
using AgronicaNetCore.Zoo.DAL.DataLayer.Ricette_ZooxAgenda;
using AgronicaNetCore.Operazione.DAL.DataLayer.RicettexAgenda;
using AgronicaNetCore.OperazioniZoo.BIZ.Services.OperazioniZoo;
using AgronicaNetCore.OperazioniZoo.BIZ.Services.Trattamenti;
using AgronicaNetCore.OperazioniZoo.DAL.DataLayer.OperazioniZoo;
using AgronicaNetCore.ProfilazioneImprese.BIZ.Services;
using AgronicaNetCore.ProfilazioneImprese.DAL.DataLayer.ProfilazioneImprese;
using AgronicaNetCore.ProfilazioneMacchine.BIZ.Services;
using AgronicaNetCore.ProfilazioneMacchine.DAL.DataLayer.ProfilazioneMacchine;
using AgronicaNetCore.SpecieVegetaliDefault.DAL.DataLayer.SpecieVegetaliDefault;
using AgronicaNetCore.SuperServer.BIZ.Autenticazione;
using AgronicaNetCore.SuperServer.DAL.Autenticazione;
using AgronicaNetCore.Utenti.BIZ.Services.UtentiImpostazioni;
using AgronicaNetCore.Utenti.BIZ.Services.UtentiImpostazioniFiltroMono;
using AgronicaNetCore.Utenti.BIZ.Services.UtentiProfili;
using AgronicaNetCore.Utenti.BIZ.Services.UtentiVisibilitaAppoggio;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiDettagli;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiImpostazioni;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiImpostazioniFiltroMono;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiProfili;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiVisibilitaAppoggio;
using AgronicaNetCore.UtilityDB.BIZ.Services.UtilityDB;
using AgronicaNetCore.UtilityDB.DAL.DataLayer.Agro_Sequenze;
using AgronicaNetCore.UtilityDB.DAL.DataLayer.UtilityDB;
using AgronicaNetCore.Widgets.BIZ.Services.WidgetStatistiche;
using AgronicaNetCore.Widgets.DAL.DataLayer.WidgetsDocumentale.WidgetsDocumentale;
using AgronicaNetCore.Widgets.DAL.DataLayer.WidgetsStatistiche.WidgetsStatistiche;
using AgronicaNetCore.Zoo.BIZ.Services.Prescrizioni;
using AgronicaNetCore.Zoo.DAL.DataLayer.Prescrizioni;
using AgronicaNetCore.Logging.DAL.DataLayer.AgronicaLogInvio.AgronicaLogInvioAgenda;
using AgronicaNetCore.Logging.DAL.DataLayer.AgronicaLogInvio.AgronicaLogInvioChiamate;
using AgronicaNetCore.Logging.BIZ.Services.AgronicaLogInvio.AgronicaLogInvioAgenda;
using AgronicaNetCore.Logging.BIZ.Services.AgronicaLogInvio.AgronicaLogInvioChiamate;
using Humanizer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using AgronicaNetCore.Utility.BIZ.Services.AgroZip;
using AgronicaNetCore.Operazione.BIZ.Services.ActivityImport.Mapper;
using AgronicaCoreDTOStd.InData.ActivityImport;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.Esercizi;
using AgronicaNetCore.Operazione.DAL.Resources.UtilityAgenda;
using AgronicaNetCore.Operazione.BIZ.Services.ActivityImport;
using AgronicaNetCore.Operazione.BIZ.Services.Agenda.Mapper;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.Lavorazione;
using AgronicaNetCore.Operazione.BIZ.Services.Movimenti.Factory;
using AgronicaNetCore.Operazione.BIZ.Services.Movimenti_Dettagli.Factory;
using AgronicaNetCore.Operazione.BIZ.Services.Mov_Destinazioni.Factory;
using AgronicaNetCore.Operazione.BIZ.Services.Agenda;
using AgronicaNetCore.Operazione.DAL.DataLayer.Agenda;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti;
using AgronicaNetCore.Operazione.DAL.DataLayer.Agronica_Log_Agenda;
using AgronicaNetCore.Operazione.BIZ.Services.Movimenti;
using AgronicaNetCore.Operazione.BIZ.Services.Movimenti_Dettagli;
using AgronicaNetCore.Operazione.BIZ.Services.Mov_Destinazioni;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti_Dettagli;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti_Zoo;
using AgronicaNetCore.Operazione.BIZ.Services.Movimenti_Zoo;
using AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Dettaglio_Tecnico;
using AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Dettaglio_Tecnico_Extra;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.Apezzamenti;
using AgronicaNetCore.Zoo.DAL.DataLayer.Ricette_Zoo;
using AgronicaNetCore.Zoo.DAL.DataLayer.Ricette_Zoo_Agenda;
using AgronicaNetCore.Zoo.DAL.DataLayer.Ricette_Zoo_Movimenti;
using AgronicaNetCore.Zoo.DAL.DataLayer.Ricette_Zoo_Dettagli;
using AgronicaNetCore.Zoo.DAL.DataLayer.Ricette_Zoo_Destinazioni;
using AgronicaNetCore.Zoo.DAL.DataLayer.Ricette_Zoo_Dettaglio_Tecnico;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.Imprese_Impostazioni;
using AgronicaNetCore.Documentale.DAL.DataLayer;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.UnitaMisura;
using AgronicaNetCore.Documentale.BIZ.Services;
using AgronicaNetCore.Utility.DAL.DataLayer;
using AgronicaNetCore.Utility.DAL.DataLayer.ObjectUtility;

namespace AgronicaNetCoreApi.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, ILoggingBuilder logging, IWebHostEnvironment env)
        {
            #region "Logger"
            logging.ClearProviders();
            logging.AddSerilog(new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .WriteTo.Logger(c1 => c1
                                            .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("LogPath"))
                                            .WriteTo.Map("LogPath", (pathLog, wt) => wt.File($"{pathLog}-.log", rollingInterval: RollingInterval.Day, shared: true))
                                            )
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                            .CreateLogger());
            #endregion

            #region "Localization"
            services.AddLocalization(options => options.ResourcesPath = "");
            #endregion


            var connectionString = configuration.GetValue<string>("ConnectionString");

            // decodifica la stringa di connessione se criptata
            if (configuration.GetValue<bool>("ConnectionStringEncoded"))
            {
                connectionString = Security.DecryptString(connectionString, configuration.GetValue<string>("cr2"));
            }

            #region "Security"
            //0. abilitazione DataProtection per sharing crypt key for cookies
            services.AddDbContext<AgronicaDataProtectionContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });

            services.AddDataProtection()
                    .SetApplicationName("Agronica.Auth")
                .PersistKeysToDbContext<AgronicaDataProtectionContext>();

            // leggo la chiave jwt dalle varibili di ambiente se presente altrimenti dall'appsettings
            var key = Environment.GetEnvironmentVariable(configuration["Jwt:KeyName"] ?? "JWT_KEY") ?? configuration["Jwt:Key"] ??
                throw new ApplicationException("JWT key is not configured.");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            //1 - Authentication & JWT/Cookies
            services
                    .AddAuthentication(o =>
                    {
                        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration["Jwt:Issuer"],
                            ValidAudience = configuration["Jwt:Issuer"],
                            IssuerSigningKey = securityKey
                        };
                    })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                    {
                        o.ExpireTimeSpan = TimeSpan.FromMinutes(240); // optional
                        o.Cookie.Name = "auth_cookie";
                        o.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                        o.Cookie.Path = "/";
                    });

            services.AddControllers().AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.Converters.Add(new StringEnumConverter());
                // x.SerializerSettings.Converters.Add(new AttivitaConverter());
                // x.SerializerSettings.Converters.Add(new UtilizzoTerrenoConverter());
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                x.SerializerSettings.ContractResolver = new DefaultContractResolver();
                x.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                x.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                // x.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AgronicaCoreApiNet6", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "AgronicaCoreDTOStd.xml"), includeControllerXmlComments: true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "AgronicaCoreModelsSTD.xml"), includeControllerXmlComments: true);
                // Aggiungere documentazione swagger
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
                // To Enable authorization using Swagger (JWT)
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.CustomSchemaIds(x => x.ToString());
                c.CustomOperationIds(description => description.TryGetMethodInfo(out MethodInfo methodInfo) ? $"{methodInfo.DeclaringType!.Name.Replace("Controller", string.Empty)}{methodInfo.Name.Humanize().Pascalize()}" : null);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            }).AddSwaggerGenNewtonsoftSupport();

            //- Authorization Linked to Authenticated
            var policy = new AuthorizationPolicyBuilder(
                                                    CookieAuthenticationDefaults.AuthenticationScheme,
                                                    JwtBearerDefaults.AuthenticationScheme)
                                                  .RequireAuthenticatedUser()
                                                  .Build();
            services.AddAuthorization(o => o.DefaultPolicy = policy);
            #endregion

            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowOrigin",
                    builder =>
                    {
                        //Lavez - 06/03/2025 - Definizione della policy di CORS, la quale è abilitata SOLO se il flag "allowCORS" in appsettings è true
                        builder.AllowAnyOrigin();
                        builder.WithOrigins("https://localhost:44320", "https://localhost:4200", "http://localhost:4200");
                        builder.WithMethods("POST", "GET", "OPTIONS");
                        //builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        //builder.AllowAnyMethod();
                        builder.AllowCredentials();
                    });
            });


            #region "Services"
            // Add services to the container.

            //Lavez - 30/09/2024 - Parametri per pilotare alcune impostazioni relative alla gestione sql
            services.Configure<SqlSettings>(configuration.GetSection("SqlSettings"));

            //parametri relativi alla sicurezza
            services.Configure<SecuritySettings>(configuration.GetSection("SecuritySettings"));

            // security service
            services.AddSingleton<ISecurityService, SecurityService>();
            // security service
            services.AddSingleton<ICultureService, CultureService>();
            //Generali
            services.AddScoped<IIdentityService, IdentityService>();

            //DataProviderFactory
            services.AddSingleton<IDataProvider6Factory, DataProvider6Factory>();

            #region "DAL"
            services.AddTransient<IAgro_Sequence, Agro_Sequence>();
            services.AddTransient<IImpresa, Impresa>();
            services.AddTransient<IImpianti, Impianti>();
            services.AddTransient<IAppezzamenti, Appezzamenti>();
            services.AddTransient<ILavorazione, Lavorazione>();
            services.AddTransient<IEsercizi, Esercizi>();
            services.AddTransient<AgronicaNetCore.Utility.DAL.DataLayer.FiltriTabelle.ITmpAgenda, AgronicaNetCore.Utility.DAL.DataLayer.FiltriTabelle.TmpAgenda>();
            services.AddTransient<IProgrammazione, Programmazione>();
            services.AddTransient<IWidgetsStatistiche, WidgetsStatistiche>();
            services.AddTransient<IPianoColturale, PianoColturale>();
            services.AddTransient<IUtentiImpostazioni, UtentiImpostazioni>();
            services.AddTransient<IWidgetsDocumentale, WidgetsDocumentale>();
            services.AddTransient<IUtentiProfili, UtentiProfili>();
            services.AddTransient<IFiltroRicerca, FiltroRicerca>();
            services.AddTransient<IUtentiVisibilitaAppoggio, UtentiVisibilitaAppoggio>();
            services.AddTransient<ISpecieVegetali, SpecieVegetali>();
            services.AddTransient<IDivisioniAmministrative, DivisioniAmministrative>();
            services.AddTransient<IZone, Zone>();
            services.AddTransient<ILettureStatiche, LettureStatiche>();
            services.AddTransient<IUnitaMisuraConversione, UnitaMisuraConversione>();
            services.AddTransient<IUtilityDB, UtilityDB>();
            services.AddTransient<IUtentiImpostazioniFiltroMono, UtentiImpostazioniFiltroMono>();
            services.AddTransient<ICodiciAnagrafe, CodiciAnagrafe>();
            services.AddTransient<IServizi, Servizi>();
            services.AddTransient<IGis, Gis>();
            services.AddTransient<IOperazioni, Operazioni>();
            services.AddTransient<IGruppiOperazione, GruppiOperazione>();
            services.AddTransient<IParcoMacchine, ParcoMacchine>();
            services.AddTransient<IContatti, Contatti>();
            services.AddTransient<IBudget, Budget>();
            services.AddTransient<IGerarchiaImprese, GerarchiaImprese>();
            services.AddTransient<IUtentiDettagli, UtentiDettagli>();
            services.AddTransient<IProfilazioneImprese, ProfilazioneImprese>();
            services.AddTransient<IProfilazioneMacchine, ProfilazioneMacchine>();
            services.AddTransient<IOperazione, Operazione>();
            services.AddTransient<IUtilityAgenda, UtilityAgenda>();
            services.AddTransient<IUtilityAgendaClassInitializer, UtilityAgendaClassInitializer>();
            services.AddTransient<IMacchineCaratteristiche, MacchineCaratteristiche>();
            services.AddTransient<IDitte, Ditte>();
            services.AddTransient<IUnitaMisura, UnitaMisura>();
            services.AddTransient<INote, Note>();
            services.AddTransient<ISpecieVegetaliDefault, SpecieVegetaliDefault>();
            services.AddTransient<IAutenticazione, Autenticazione>();
            services.AddTransient<IPrincipiAttivi, PrincipiAttivi>();
            services.AddTransient<IOperazioneCausale, OperazioneCausale>();
            services.AddTransient<IContatoriAcqua, ContatoriAcqua>();
            services.AddTransient<IReportImpiegoProdottiFitosanitari, ReportImpiegoProdottiFitosanitari>();
            services.AddTransient<ISecurityLayer, SecurityLayer>();
            services.AddTransient<IOperazioniZoo, OperazioniZoo>();
            services.AddTransient<IPrescrizioni, Prescrizioni>();
            services.AddTransient<IFarmaci, Farmaci>();
            services.AddTransient<IAgronicaLogInvioAgenda, AgronicaLogInvioAgenda>();
            services.AddTransient<IAgronicaLogInvioChiamate, AgronicaLogInvioChiamate>();
            services.AddTransient<IAgronica_Log_Agenda, Agronica_Log_Agenda>();
            services.AddTransient<IMovimenti_Zoo, Movimenti_Zoo>();
            services.AddTransient<AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Destinazioni.IMov_Destinazioni, AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Destinazioni.Mov_Destinazioni>();
            services.AddTransient<IMov_Dettaglio_Tecnico_Extra, Mov_Dettaglio_Tecnico_Extra>();
            services.AddTransient<IMovimenti_Dettagli, MovimentiDettagli>();
            services.AddTransient<IMovimenti, Movimenti>();
            services.AddTransient<IAgenda, Agenda>();
            services.AddTransient<ITrattamenti, Trattamenti>();
            services.AddTransient<IAgenda, Agenda>();
            services.AddTransient<IAgronica_Log_Agenda, Agronica_Log_Agenda>();
            services.AddTransient<IRicette, Ricette>();
            services.AddTransient<IRicettexAgenda, RicettexAgenda>();
            services.AddTransient<IRicette_ZooxAgenda, Ricette_ZooxAgenda>();
            services.AddTransient<IMovimenti, Movimenti>();
            services.AddTransient<IMovimenti_Dettagli, MovimentiDettagli>();
            services.AddTransient<IMov_Destinazioni, Mov_Destinazioni>();
            services.AddTransient<IMovimenti_Zoo, Movimenti_Zoo>();
            services.AddTransient<IMov_Dettaglio_Tecnico, Mov_Dettaglio_Tecnico>();
            services.AddTransient<IMov_Dettaglio_Tecnico_Extra, Mov_Dettaglio_Tecnico_Extra>();
            services.AddTransient<IFabbricati, Fabbricati>();
            services.AddTransient<IRicette_Zoo, Ricette_Zoo>();
            services.AddTransient<IRicette_Zoo_Agenda, Ricette_Zoo_Agenda>();
            services.AddTransient<IRicette_Zoo_Movimenti, Ricette_Zoo_Movimenti>();
            services.AddTransient<IRicette_Zoo_Dettagli, Ricette_Zoo_Dettagli>();
            services.AddTransient<IRicette_Zoo_Destinazioni, Ricette_Zoo_Destinazioni>();
            services.AddTransient<IRicette_Zoo_Dettaglio_Tecnico, Ricette_Zoo_Dettaglio_Tecnico>();
            services.AddTransient<IImprese_Impostazioni, Imprese_Impostazioni>();
            services.AddTransient<IMagazzino, Magazzino>();
            services.AddTransient<IExportDocumenti, ExportDocumenti>();
            services.AddTransient<ICompressioneDecompressione, CompressioneDecompressione>();
            #endregion

            #region "BIZ"
            services.AddScoped<IConfrontoCatastoService, ConfrontoCatastoService>();
            services.AddScoped<IImpresaService, ImpresaService>();
            services.AddScoped<IWidgetStatisticheService, WidgetStatisticheService>();
            services.AddScoped<IPianoColturaleService, PianoColturaleService>();
            services.AddScoped<IWidgetDocumentaleService, WidgetDocumentaleService>();
            services.AddScoped<IFiltroRicercaService, FiltroRicercaService>();
            services.AddScoped<ISpecieVegetaliService, SpecieVegetaliService>();
            services.AddScoped<IUtentiImpostazioniFiltroMonoService, UtentiImpostazioniFiltroMonoService>();
            services.AddScoped<IDivisioniAmministrativeService, DivisioniAmministrativeService>();
            services.AddScoped<IZoneService, ZoneService>();
            services.AddScoped<ILettureStaticheService, LettureStaticheService>();
            services.AddScoped<IUtilityDBService, UtilityDBService>();
            services.AddScoped<IUtentiVisibilitaAppoggioService, UtentiVisibilitaAppoggioService>();
            services.AddScoped<IUtentiImpostazioniService, UtentiImpostazioniService>();
            services.AddScoped<IUnitaMisuraConversioneService, UnitaMisuraConversioneService>();
            services.AddScoped<ICodiciAnagrafeService, CodiciAnagrafeService>();
            services.AddScoped<IUtentiProfiliService, UtentiProfiliService>();
            services.AddScoped<IServiziService, ServiziService>();
            services.AddScoped<IGisService, GisService>();
            services.AddScoped<IOperazioniService, OperazioniService>();
            services.AddScoped<IProfilazioneImpreseService, ProfilazioneImpreseService>();
            services.AddScoped<IGruppiOperazioneService, GruppiOperazioneService>();
            services.AddScoped<IParcoMacchineService, ParcoMacchineService>();
            services.AddScoped<IContattiService, ContattiService>();
            services.AddTransient<IBudgetService, BudgetService>();
            services.AddScoped<IAlberoGerarchiaImpreseFASTService, AlberoGerarchiaImpreseFASTService>();
            services.AddScoped<IProfilazioneMacchineService, ProfilazioneMacchineService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IDefaultPianiColturaliService, DefaultPianiColturaliService>();
            services.AddScoped<IAutenticazioneService, AutenticazioneService>();
            services.AddScoped<IReportImpiegoProdottiFitosanitariService, ReportImpiegoProdottiFitosanitariService>();
            services.AddScoped<IOperazioneCausaleService, OperazioneCausaleService>();
            services.AddTransient<IContatoriAcquaService, ContatoriAcquaService>();
            services.AddTransient<IPrescrizionIService, PrescrizioniService>();
            services.AddScoped<IOperazioniZooService, OperazioniZooService>();
            services.AddScoped<ITrattamentoZooService, TrattamentoZooService>();
            services.AddScoped<IFarmaciService, FarmaciService>();
            services.AddScoped<IImpiantiService, ImpiantiService>();
            services.AddScoped<IAgronicaLogInvioAgendaService, AgronicaLogInvioAgendaService>();
            services.AddScoped<IAgronicaLogInvioChiamateService, AgronicaLogInvioChiamateService>();
            services.AddScoped<IAgroZipService, AgroZipService>();
            //services.AddTransient<IActivityImportPlant, ActivityImportPlant>();
            //services.AddTransient<IMovZooService, MovZooService>();
            services.AddTransient<IMovDestinazioniFactory, MovDestinazioniFactory>();
            services.AddTransient<IMovDestinazioniService, MovDestinazioniService>();
            services.AddTransient<IMov_Dettaglio_Tecnico, Mov_Dettaglio_Tecnico>();
            services.AddTransient<IMovDettagliFactory, MovDettagliFactory>();
            services.AddTransient<IMovDettagliService, MovDettagliService>();
            services.AddTransient<IMovimentiFactory, MovimentiFactory>();
            services.AddTransient<IMovimentiService, MovimentiService>();
            services.AddTransient<IAgendaService, AgendaService>();
            services.AddTransient<IAttivitaToAgenda, AttivitaToAgenda>();
            services.AddTransient<IActivityImportData, ActivityImportData>();
            services.AddTransient<IActivityImportService, ActivityImportService>();

            services.AddTransient(typeof(IActivityImportMapper<IActivityImportData>),typeof(ActivityImportMapperOrogel<IActivityImportData>));
            //// To add more mappers, add the declaration as follows:
            //services.AddTransient(typeof(IActivityImportMapper<IActivityImportDataExample2>),typeof(ActivityImportMapperExample2<IActivityImportDataExample2>));

            services.AddScoped<ITrattamentiService, TrattamentiService>();
            services.AddScoped<IMagazzinoService, MagazzinoService>();
            services.AddScoped<IExportDocumentiService, ExportDocumentiService>();
            #endregion

            #endregion

            //cache
            services.AddMemoryCache();

            

            return services;
        }
    }
}
