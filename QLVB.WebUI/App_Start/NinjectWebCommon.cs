[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(QLVB.WebUI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(QLVB.WebUI.App_Start.NinjectWebCommon), "Stop")]

namespace QLVB.WebUI.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using QLVB.Core.Contract;
    using QLVB.Core.Implementation;
    using QLVB.Domain.Abstract;
    using QLVB.DAL;

    using Store.DAL.Abstract;
    using Store.DAL.Implementation;


    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            #region QLVB
            // fix loi different context
            kernel.Bind<QLVBDatabase>().ToSelf().InRequestScope();

            //=============================================
            // Domain ==> DAL
            //=============================================
            kernel.Bind<ICanboRepository>().To<EFCanboRepository>();
            kernel.Bind<IMenuRepository>().To<EFMenuRepository>();
            kernel.Bind<IQuyenRepository>().To<EFQuyenRepository>();
            kernel.Bind<IQuyenNhomQuyenRepository>().To<EFQuyenNhomQuyenRepository>();
            kernel.Bind<IHomeRepository>().To<EFHomeRepository>();
            kernel.Bind<IUyQuyenRepository>().To<EFUyQuyenRepository>();
            kernel.Bind<IDonvitructhuocRepository>().To<EFDonvitructhuocRepository>();
            kernel.Bind<IConfigRepository>().To<EFConfigRepository>();
            kernel.Bind<INhomQuyenRepository>().To<EFNhomQuyenRepository>();
            kernel.Bind<IChucdanhRepository>().To<EFChucdanhRepository>();
            kernel.Bind<INhatkyRepository>().To<EFNhatkyRepository>();
            kernel.Bind<INlogErrorRepository>().To<EFNlogErrorRepository>();

            kernel.Bind<IPhanloaiVanbanRepository>().To<EFPhanloaiVanbanRepository>();
            kernel.Bind<IPhanloaiTruongRepository>().To<EFPhanloaiTruongRepository>();
            kernel.Bind<IMotaTruongRepository>().To<EFMotaTruongRepository>();
            kernel.Bind<ISoVanbanRepository>().To<EFSoVanbanRepository>();
            kernel.Bind<IKhoiphathanhRepository>().To<EFKhoiphathanhRepository>();
            kernel.Bind<ISovbKhoiphRepository>().To<EFSovbKhoiphRepository>();
            kernel.Bind<IDiachiluutruRepository>().To<EFDiachiluutruRepository>();
            kernel.Bind<ILinhvucRepository>().To<EFLinhvucRepository>();
            kernel.Bind<ITinhchatvanbanRepository>().To<EFTinhchatvanbanRepository>();
            kernel.Bind<ITochucdoitacRepository>().To<EFTochucdoitacRepository>();

            kernel.Bind<IVanbandenRepository>().To<EFVanbandenRepository>();
            kernel.Bind<ICategoryRepository>().To<EFCategoryVanbanRepository>();
            kernel.Bind<IAttachVanbanRepository>().To<EFAttachVanbanRepository>();
            kernel.Bind<IHosocongviecRepository>().To<EFHosocongviecRepository>();

            kernel.Bind<IHosovanbanRepository>().To<EFHosovanbanRepository>();
            kernel.Bind<IHosovanbanlienquanRepository>().To<EFHosovanbanlienquanRepository>();

            kernel.Bind<IDoituongxulyRepository>().To<EFDoituongxulyRepository>();
            kernel.Bind<IHoibaovanbanRepository>().To<EFHoibaovanbanRepository>();
            kernel.Bind<IVanbandiRepository>().To<EFVanbandiRepository>();
            kernel.Bind<IChitietVanbandenRepository>().To<EFChitietVanbandenRepository>();
            kernel.Bind<IVanbandenCanboRepository>().To<EFVanbandenCanboRepository>();
            kernel.Bind<IVanbandiCanboRepository>().To<EFVanbandiCanboRepository>();

            kernel.Bind<IChitietHosoRepository>().To<EFChitietHosoRepository>();
            kernel.Bind<IChitietVanbandiRepository>().To<EFChitietVanbandiRepository>();
            kernel.Bind<IHosoykienxulyRepository>().To<EFHosoykienxulyRepository>();
            kernel.Bind<IBaocaoRepository>().To<EFBaocaoRepository>();
            kernel.Bind<IGuiVanbanRepository>().To<EFGuiVanbanRepository>();
            kernel.Bind<IPhieutrinhRepository>().To<EFPhieutrinhRepository>();
            kernel.Bind<IAttachHosoRepository>().To<EFAttachHosoRepository>();
            kernel.Bind<IAttachMailRepository>().To<EFAttachMailRepository>();
            kernel.Bind<ITonghopCanboRepository>().To<EFTonghopCanboRepository>();

            kernel.Bind<IMailInboxRepository>().To<EFMailInboxRepository>();
            kernel.Bind<IMailOutboxRepository>().To<EFMailOutboxRepository>();
            kernel.Bind<IVanbandenmailRepository>().To<EFVanbandenmailRepository>();
            kernel.Bind<IPhanloaiQuytrinhRepository>().To<EFPhanloaiQuytrinhRepository>();
            kernel.Bind<IQuytrinhRepository>().To<EFQuytrinhRepository>();
            kernel.Bind<IQuytrinhNodeRepository>().To<EFQuytrinhNodeRepository>();
            kernel.Bind<IQuytrinhConnectionRepository>().To<EFQuytrinhConnectionRepository>();
            kernel.Bind<IQuytrinhXulyRepository>().To<EFQuytrinhXulyRepository>();
            kernel.Bind<IHosoQuytrinhXulyRepository>().To<EFHosoQuytrinhxulyRepository>();
            kernel.Bind<IQuytrinhVersionRepository>().To<EFQuytrinhVersionRepository>();
            kernel.Bind<IHosoQuytrinhRepository>().To<EFHosoQuytrinhRepository>();

            kernel.Bind<ISqlQuery>().To<EFSqlQuery>();

            kernel.Bind<ITuychonRepository>().To<EFTuychonRepository>();
            kernel.Bind<ITuychonCanboRepository>().To<EFTuychonCanboRepository>();

            // view
            kernel.Bind<ITinhtrangxulyRepository>().To<EFTinhtrangxulyRepository>();
            kernel.Bind<ITinhtrangQuytrinhRepository>().To<EFTinhtrangQuytrinhRepository>();

            //===============================================
            // Core
            //===============================================

            kernel.Bind<QLVB.Common.Logging.ILogger>().To<QLVB.WebUI.Common.NLog.NLogLogger>();

            kernel.Bind<IRoleManager>().To<RoleManager>();
            kernel.Bind<IMenuManager>().To<MenuManager>();
            kernel.Bind<IAccountManager>().To<AccountManager>();
            kernel.Bind<IHethongManager>().To<HethongManager>();
            kernel.Bind<ILogManager>().To<LogManager>();
            kernel.Bind<IChucdanhManager>().To<ChucdanhManager>();
            kernel.Bind<ILoaivanbanManager>().To<LoaivanbanManager>();
            kernel.Bind<IDonviManager>().To<DonviManager>();
            kernel.Bind<ISovanbanManager>().To<SovanbanManager>();
            kernel.Bind<ILuutruManager>().To<LuutruManager>();
            kernel.Bind<ILinhvucManager>().To<LinhvucManager>();
            kernel.Bind<ITinhchatvanbanManager>().To<TinhchatvanbanManager>();
            kernel.Bind<ICapcoquanManager>().To<CapcoquanManager>();
            kernel.Bind<IVanbandenManager>().To<VanbandenManager>();
            kernel.Bind<IFileManager>().To<FileManager>();
            kernel.Bind<IVanbandiManager>().To<VanbandiManager>();
            kernel.Bind<IHosoManager>().To<HosoManager>();
            kernel.Bind<IBaocaoManager>().To<BaocaoManager>();
            kernel.Bind<IReportManager>().To<ReportManager>();
            kernel.Bind<IRuleFileNameManager>().To<RuleFileNameManager>();
            kernel.Bind<ITinhhinhxulyManager>().To<TinhhinhxulyManager>();

            //kernel.Bind<IMailManager>().To<MailActiveManager>();
            kernel.Bind<IMailManager>().To<MailBeeManager>();

            kernel.Bind<IMailFormatManager>().To<MailFormatManager>();
            kernel.Bind<ITonghopManager>().To<TonghopManager>();
            kernel.Bind<IVanbandientuManager>().To<VanbandientuManager>();
            kernel.Bind<IQuytrinhManager>().To<QuytrinhManager>();

            kernel.Bind<IEdxmlManager>().To<EdxmlManager>();
            // session owin
            kernel.Bind<QLVB.Common.Sessions.ISessionServices>().To<QLVB.WebUI.Common.Session.SessionOwin>();

            //signalR
            kernel.Bind<QLVB.WebUI.Hubs.IChatRepository>().To<QLVB.WebUI.Hubs.ChatRepository>();

            #endregion QLVB

            #region YKCD
            //===============================================
            // YKCD Core
            //===============================================
            kernel.Bind<IYKCDDonviManager>().To<YKCDDonviManager>();

            #endregion YKCD

            #region Store

            // fix loi different context
            kernel.Bind<Store.DAL.QLVBStoreDatabase>().ToSelf().InRequestScope();

            //==========================================================
            // Store.DAL.Abstract ==> Store.DAL.Implementaton
            //==========================================================

            kernel.Bind<Store.DAL.Abstract.IStoreVanbandenRepository>().To<Store.DAL.Implementation.EFStoreVanbandenRepository>();
            kernel.Bind<IStoreAttachHosoRepository>().To<EFStoreAttachHosoRepository>();
            kernel.Bind<IStoreAttachVanbanRepository>().To<EFStoreAttachVanbanRepository>();
            kernel.Bind<IStoreDoituongxulyRepository>().To<EFStoreDoituongxulyRepository>();
            kernel.Bind<IStoreHosocongviecRepository>().To<EFStoreHosocongviecRepository>();
            kernel.Bind<IStoreHosovanbanRepository>().To<EFStoreHosovanbanRepository>();
            kernel.Bind<IStoreHosovanbanlienquanRepository>().To<EFStoreHosovanbanlienquanRepository>();
            kernel.Bind<IStoreHosoykienxulyRepository>().To<EFStoreHosoykienxulyRepository>();
            kernel.Bind<IStorePhieutrinhRepository>().To<EFStorePhieutrinhRepository>();
            kernel.Bind<IStoreVanbandenCanboRepository>().To<EFStoreVanbandenCanboRepository>();
            kernel.Bind<IStoreVanbandiRepository>().To<EFStoreVanbandiRepository>();
            kernel.Bind<IStoreVanbandiCanboRepository>().To<EFStoreVanbandiCanboRepository>();
            kernel.Bind<IStoreHoibaovanbanRepository>().To<EFStoreHoibaovanbanRepository>();
            kernel.Bind<IStoreChitietHosoRepository>().To<EFStoreChitietHosoRepository>();

            //===============================================
            // Store.Core
            //===============================================
            kernel.Bind<Store.Core.Contract.ITracuuVanbandenManager>().To<Store.Core.Implementation.TracuuVanbandenManager>();
            kernel.Bind<Store.Core.Contract.IStoreFileManager>().To<Store.Core.Implementation.StoreFileManager>();
            kernel.Bind<Store.Core.Contract.ITracuuVanbandiManager>().To<Store.Core.Implementation.TracuuVanbandiManager>();
            kernel.Bind<Store.Core.Contract.ITracuuHosoManager>().To<Store.Core.Implementation.TracuuHosoManager>();

            #endregion Store

        }
    }
}
