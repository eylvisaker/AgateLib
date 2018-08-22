using AgateLib.UserInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Rendering.Animations
{
    /// <summary>
    /// Interface for a factory which creates animation objects.
    /// </summary>
    public interface IAnimationFactory
    {
        /// <summary>
        /// Creates an animation object of the specified name. 
        /// This method should always return a valid animation object, even
        /// if the name of an animation is not recognized.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        IWidgetAnimation Create(string name, IReadOnlyList<string> args);
    }

    /// <summary>
    /// Standard animation factory.
    /// </summary>
    [Singleton]
    public class AnimationFactory : IAnimationFactory
    {
        Dictionary<string, Func<IReadOnlyList<string>, IWidgetAnimation>> activators
            = new Dictionary<string, Func<IReadOnlyList<string>, IWidgetAnimation>>(StringComparer.OrdinalIgnoreCase);

        public AnimationFactory()
        {
            AddAnimationActivator("default", _ => new NullAnimation());
            AddAnimationActivator("fade", args => new FadeAnimation(args));
        }

        /// <summary>
        /// Creates an animation.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IWidgetAnimation Create(string name, IReadOnlyList<string> args)
        {
            name = name ?? "default";

            if (!activators.ContainsKey(name))
            {
                Log.Warn($"Animation named {name} not found.");
                name = "default";
            }

            return activators[name].Invoke(args);
        }

        /// <summary>
        /// Adds an animation factory method to the library.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="activator"></param>
        public void AddAnimationActivator(string name, Func<IReadOnlyList<string>, IWidgetAnimation> activator)
        {
            activators[name] = activator;
        }
    }

    public static class AnimationFactoryExtensions
    {
        public static void Configure(this IAnimationFactory animationFactory, RenderElementDisplay display)
        {
            if (display.Style.Animation == null)
            {
                display.Animation.In = display.Animation.In ?? animationFactory.Create("default", null);
                display.Animation.Out = display.Animation.Out ?? animationFactory.Create("default", null);
                display.Animation.Static = display.Animation.Static ?? animationFactory.Create("default", null);

                return;
            }

            // Currently, we don't support updating animators when the style changes unless the name of the
            // animator type has changed. I don't see a good use case for supporting this at the moment. -EY
            if (display.Animation.InType != display.Style.Animation.EntryName)
            {
                display.Animation.InType = display.Style.Animation.EntryName;
                display.Animation.In = animationFactory.Create(display.Style.Animation.EntryName, display.Style.Animation.EntryArgs);
            }

            if (display.Animation.OutType != display.Style.Animation.ExitName)
            {
                display.Animation.OutType = display.Style.Animation.ExitName;
                display.Animation.Out = animationFactory.Create(display.Style.Animation.ExitName, display.Style.Animation.ExitArgs);
            }

            if (display.Animation.StaticType != display.Style.Animation.StaticName)
            {
                display.Animation.StaticType = display.Style.Animation.StaticName;
                display.Animation.Static = animationFactory.Create(display.Style.Animation.StaticName, display.Style.Animation.StaticArgs);
            }

            if (display.Animation.In == null)
            {
                display.Animation.In = display.Animation.In ?? animationFactory.Create("default", null);
            }

            if (display.Animation.Out == null)
            {
                display.Animation.Out = display.Animation.Out ?? animationFactory.Create("default", null);
            }

            if (display.Animation.Static == null)
            {
                display.Animation.Static = display.Animation.Static ?? animationFactory.Create("default", null);
            }
        }
    }
}
