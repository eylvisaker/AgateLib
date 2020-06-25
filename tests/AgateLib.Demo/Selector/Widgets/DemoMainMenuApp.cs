using AgateLib.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.Demo.Selector.Widgets
{
    public class DemoMainMenuApp : Widget<DemoMainMenuAppProps>
    {
        public DemoMainMenuApp(DemoMainMenuAppProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            var demoGroups = Props.Demos.GroupBy(x => x.Category);

            var bookProps = new NotebookProps
            {
                Pages = demoGroups.Select(x =>
                {
                    return new NotebookPage(new NotebookPageProps
                    {
                        Title = x.Key,
                        Child = new FlexBox(new FlexBoxProps {
                            Children = {
                                new Label(new LabelProps { Text = $"Choose a {x.Key} demo" }),
                                new DemoDrawer(new DemoDrawerProps
                                {
                                    Tests = x.ToList(),
                                    OnAcceptTest = Props.OnAcceptTest,
                                })
                            }
                        })
                    });
                }).ToList()
            };

            return new App(new AppProps
            {
                Children = {
                    new Window(new WindowProps
                    {
                        Name = "demo-selector-app",
                        Children = { new Notebook(bookProps) }
                    })
                }
            });
        }
    }
    
    public class DemoMainMenuAppProps : WidgetProps
    {
        public List<IDemo> Demos { get; set; }

        public Action<IDemo> OnAcceptTest { get; set; }
    }
}
