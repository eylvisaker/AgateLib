using NLog;
using System;
using System.Collections.Generic;

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
        IRenderElementAnimation Create(string name, IReadOnlyList<string> args);
    }

    /// <summary>
    /// Standard animation factory.
    /// </summary>
    [Singleton]
    public class AnimationFactory : IAnimationFactory
    {
        private readonly Logger log;

        private Dictionary<string, Func<IReadOnlyList<string>, IRenderElementAnimation>> activators
            = new Dictionary<string, Func<IReadOnlyList<string>, IRenderElementAnimation>>(StringComparer.OrdinalIgnoreCase);

        public AnimationFactory()
        {
            log = LogManager.GetCurrentClassLogger();

            AddAnimationActivator("default", _ => new NullAnimation());
            AddAnimationActivator("fade", args => new FadeAnimation(args));
            AddAnimationActivator("slide", args => new SlideAnimation(args));
        }

        /// <summary>
        /// Creates an animation.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IRenderElementAnimation Create(string name, IReadOnlyList<string> args)
        {
            name = name ?? "default";

            if (!activators.ContainsKey(name))
            {
                log.Warn($"Animation named {name} not found.");
                name = "default";
            }

            return activators[name].Invoke(args);
        }

        /// <summary>
        /// Adds an animation factory method to the library.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="activator"></param>
        public void AddAnimationActivator(string name, Func<IReadOnlyList<string>, IRenderElementAnimation> activator)
        {
            activators[name] = activator;
        }
    }

    public static class AnimationFactoryExtensions
    {
        public static void Configure(this IAnimationFactory animationFactory, RenderElementDisplay display)
        {
            Configure(animationFactory, display.Animation, display.Style);
        }

        private static void Configure(IAnimationFactory animationFactory,
                                      RenderElementAnimator animation,
                                      IRenderElementStyle style)
        {
            if (style.Animation == null)
            {
                animation.In = animation.In ?? animationFactory.Create("default", null);
                animation.Out = animation.Out ?? animationFactory.Create("default", null);
                animation.Static = animation.Static ?? animationFactory.Create("default", null);

                return;
            }

            // Currently, we don't support updating animators when the style changes unless the name of the
            // animator type has changed. I don't see a good use case for supporting this at the moment. -ERY
            if (animation.InType != style.Animation.EntryName)
            {
                animation.InType = style.Animation.EntryName;
                animation.In = animationFactory.Create(style.Animation.EntryName, style.Animation.EntryArgs);
            }

            if (animation.OutType != style.Animation.ExitName)
            {
                animation.OutType = style.Animation.ExitName;
                animation.Out = animationFactory.Create(style.Animation.ExitName, style.Animation.ExitArgs);
            }

            if (animation.StaticType != style.Animation.StaticName)
            {
                animation.StaticType = style.Animation.StaticName;
                animation.Static = animationFactory.Create(style.Animation.StaticName, style.Animation.StaticArgs);
            }

            if (animation.In == null)
            {
                animation.In = animation.In ?? animationFactory.Create("default", null);
            }

            if (animation.Out == null)
            {
                animation.Out = animation.Out ?? animationFactory.Create("default", null);
            }

            if (animation.Static == null)
            {
                animation.Static = animation.Static ?? animationFactory.Create("default", null);
            }
        }
    }
}
