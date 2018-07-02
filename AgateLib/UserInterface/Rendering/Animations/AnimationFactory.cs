using AgateLib.UserInterface.Widgets;
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
                display.Animator.In = display.Animator.In ?? animationFactory.Create("default", null);
                display.Animator.Out = display.Animator.Out ?? animationFactory.Create("default", null);
                display.Animator.Static = display.Animator.Static ?? animationFactory.Create("default", null);

                return;
            }

            // Currently, we don't support updating animators when the style changes unless the name of the
            // animator type has changed. I don't see a good use case for supporting this at the moment. -EY
            if (display.Animator.InType != display.Style.Animation.InName)
            {
                display.Animator.InType = display.Style.Animation.InName;
                display.Animator.In = animationFactory.Create(display.Style.Animation.InName, display.Style.Animation.InArgs);
            }

            if (display.Animator.OutType != display.Style.Animation.OutName)
            {
                display.Animator.OutType = display.Style.Animation.OutName;
                display.Animator.Out = animationFactory.Create(display.Style.Animation.OutName, display.Style.Animation.OutArgs);
            }

            if (display.Animator.StaticType != display.Style.Animation.StaticName)
            {
                display.Animator.StaticType = display.Style.Animation.StaticName;
                display.Animator.Static = animationFactory.Create(display.Style.Animation.StaticName, display.Style.Animation.StaticArgs);
            }
        }
    }
}
