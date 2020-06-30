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
            return new Grid(new GridProps
            {
                Columns = 2,
                Children = {
                    new Label("Theme"), new ValueSpinner<string>(new ValueSpinnerProps<string>
                    {
                        Values = Props.AvailableThemes.Select(x => new SpinnerValue<string>(x)).ToList(),
                        OnValueChanged = e => e.System.DefaultTheme = e.Arg1,
                    }),
                    new Label("Scaling"), new ValueSpinner<float>(new ValueSpinnerProps<float>
                    {
                        Values = { 0.6f, 0.8f, 0.9f, 1, 1.25f, 1.5f, 2.0f },
                        OnValueChanged = e => e.System.VisualScaling = e.Arg1,
                        InitialValueIndex = 3,
                    })
                },
            });;
        }
    }

    public class OptionsPageProps : WidgetProps
    {
        public List<string> AvailableThemes { get; set; } = new List<string>();
    }
}