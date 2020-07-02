using App.Common.Abstraction;
using App.Common.CustomEventArgs;
using System;

namespace App.ResultsTable
{
    public class ResultsTableController : Common.Abstraction.Controller
    {
        public ResultsTableController(ResultsTableView view, ResultsTableModel model)
        {
            this.view = view;
            this.model = model;

            view.Loaded += this.View_Loaded;
            view.Closing += this.View_Closing;
        }

        protected override View View => view;
        protected override Model Model => model;

        private readonly ResultsTableView view;
        private readonly ResultsTableModel model;

        public override event EventHandler<ControllerClosedEventArgs> ControllerClosed;

        private void View_Loaded(object sender, EventArgs e)
        {
            view.FillTables(model.z, model.T, model.eta);
        }

        private void View_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.ControllerClosed.Invoke(this, new ControllerClosedEventArgs(ControllerClosedEventArgs.CloseReason.Success));
        }

        public override void Close() => view.Close();
        public override void Show() => view.ShowDialog();
    }
}
