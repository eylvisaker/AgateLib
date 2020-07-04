using AgateLib.UserInterface;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.Demo.Selector.Widgets
{
    public class OptionsPage : Widget<OptionsPageProps>
    {
        public OptionsPage(OptionsPageProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            List<float> scaleValues = new List<float> { 0.7f, 0.8f, 0.9f, 1, 1.25f, 1.5f, 1.75f, 2.0f, 2.2f, 2.4f };

            return new Grid(new GridProps
            {
                Columns = 2,
                Children = {
                    new Label("Theme"), new ValueSpinner<string>(new ValueSpinnerProps<string>
                    {
                        Values = Props.AvailableThemes.Select(x => new SpinnerValue<string>(x)).ToList(),
                        OnValueChanged = e => e.System.Theme = e.System.Config.DefaultTheme = e.Arg1,
                    }),
                    new Label("Scaling"), new ValueSpinner<float>(new ValueSpinnerProps<float>
                    {
                        Values = scaleValues.Select(x => new SpinnerValue<float>(x)).ToList(),
                        OnValueChanged = e => {
                            e.System.Config.UserScaling = e.Arg1;
                            e.System.VisualScaling = e.System.Config.VisualScaling;
                        },

                        InitialValueIndex = scaleValues.IndexOf(AppContext.Config.UserScaling),
                    })
                },
            }); ;
        }
    }

    public class OptionsPageProps : WidgetProps
    {
        public List<string> AvailableThemes { get; set; } = new List<string>();
    }
}