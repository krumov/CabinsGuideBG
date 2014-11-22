using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using MountainGuideBG.Common;
using Parse;
using MountainGuideBG.Models;
using Windows.UI.Xaml.Media.Imaging;
using System.Xml.Linq;

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace MountainGuideBG
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        /// <summary>
        /// Initializes the singleton instance of the <see cref="App"/> class. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

            ParseObject.RegisterSubclass<Cabin>();
            ParseObject.RegisterSubclass<Mountain>();
            ParseClient.Initialize("IldIO7LbSO62QJ6AhHHXWu8o1Ar6YLwnO8Ok0JRc", "b8S2QLF2uEc9Cec3juGaXB68FT4q1tJ62Gae7dwy");
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        // Something went wrong restoring state.
                        // Assume there is no state and continue
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(HubPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();

            //Cabin cabin = new Cabin();
            //cabin.Name = "Равнец";
            //cabin.Mountain = "Balkan";
            //cabin.Description = "Представлява масивна двуетажна сграда с капацитет 32 места в стаи с 2, 3, 4 и повече легла. Сградата е електрифицирана с агрегат и е водоснабдена. Отоплението е с печки на твърдо гориво. Разполага с туристическа кухня, посуда и столова. Това е най-живописната хижа - при добро време се виждат Рила, Пирин, Витоша, Родопите и цялото Карловско и Пловдивско поле.";
            //cabin.Coordinates = new ParseGeoPoint(42.663788, 24.829098);


            //Mountain mountain = new Mountain();
            //mountain.Name = "Vitosha";
            //mountain.Description = "Витоша е планина в Западна България. Най-високата ѝ точка е Черни връх (2290 m[1]). Така тя се нарежда на четвърто място по височина в България след Рила, Пирин и Стара планина.";

            //await cabin.SaveAsync();
            //await mountain.SaveAsync();

            //Cabin cabin2 = new Cabin();
            //cabin2.Name = "Добрила";
            //cabin2.Mountain = "Balkan";
            //cabin2.Description ="Хижата е разположена в Националният парк Централен Балкан на седловината между вр. Добрила и вр. Амбарица. Надморската височина на областа е 1804 м. Хижата граничи с два от най-красивите резервати в “Централен Балкан” – “Стенето” от северозапад и “Стара река” на югоизток. Построена е от туристите от град Сопот през 1928 год. По-късно е опожарена и изградена от камък през 1932 – 1933 год. Хижата и едноименния връх са наречени на легендарния Добрил войвода.В района на тази стара хижа през 70-те и 80-те години на миналия век са изградени множество бунгала. В момента хижа Добрила е модернизирана, бунгалата са ремонтирани, изградена е по голяма столова, цялата хижа е отоплена с локално парно. Възстаноена е стара сграда с изцяло ново строителство и обзаведена в пълен лукс. Разполага с 8 самостоятелни стаи с вътрешни санитарни възли и умивални. Хижата е водоснабдена и електрефицирана. Има бар-ресторант с добре уредена кухня. През зимата пристигането на хижата става чрез атракционно извозване със снегоход-Ратрак по 10 човека на курс от втора станция на въжения лифт в Сопот. Хижа Добрила е изходен пункт за хижа Дерменка и хижа Васил Левски през върховете Амбарица, Купена и Костенурката през зимата или подсичайки ги през лятото през местността Топалица. Част от маршрута попада в резерват. Природата около хижата е неописуемо красива.";
            //cabin2.Coordinates = new ParseGeoPoint(42.423995,24.454161);

            //Cabin cabin3 = new Cabin();
            //cabin3.Name = "Васил Левски";
            //cabin3.Mountain = "Balkan";
            //cabin3.Description = "Това е първата хижа в Стара планина. Разположена е на 1350 метра надморска височина в местността Голямата гюрля, над водослива на двата начални притока на Стара река.Масивна двуетажна сграда с капацитет 70 места в стаи с по 2, 4 и повече легла. Сградата е електрифицирана с ВЕЦ и е водоснабдена. Отоплява се с парно. Разполага с туристическа кухня, посуда, столова с камина, барче и кухня. Бюфет за кафе, чай, сухи плодове, ядки, сладкарски изделия и др. трайни храни, баня. Екскурзионно летуване по различни маршрути с планински водачи. Осигурена храна и нощувки.";
            //cabin3.Coordinates = new ParseGeoPoint(42.421653,24.51931);

            //Mountain mountain2 = new Mountain();
            //mountain2.Name = "Balkan";
            //mountain2.Description = "Стара планина (в Античността Хемос или Хемус[1], на старогръцки: Αίμος, на латински: Haemus; на славянски: Маторни гори; на турски: Коджабалкан или Балкан), е планинска верига в Европа, намираща се на Балканския полуостров, на която полуостровът е и кръстен. За пръв път името Стара планина се споменава през 16-ти век. Тя се простира на запад от (долината на река Тимок[2]) в Източна Сърбия до нос Емине на Черно море на изток , като по-голямата ѝ част се намира на територията на България. Стара планина е разположена по цялата дължина на България и разделя условно страната на Северна и Южна България. Най-високата ѝ точка е връх Ботев (2375,9 м). В нейното землище са обособени много природни паркове, защитени местности и един национален парк. Тя е един от най-големите центрове на ендемични и реликтни видове.[3] Поради изградената материална база, чистия въздух и високопланинския си характер, Стара планина често е предпочитана цел за туризъм. В нея са изградени 81 хижи.";

            //await cabin2.SaveAsync();
            //await cabin3.SaveAsync();
            //await mountain2.SaveAsync();
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}