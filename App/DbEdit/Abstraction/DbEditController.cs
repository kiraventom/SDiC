using App.Common.Abstraction;
using App.Common.CustomEventArgs;
using System;
using System.ComponentModel;

namespace App.DbEdit.Abstraction
{
    public abstract class DbEditController : Controller
    {
        protected DbEditController(DbEditView view, DbEditModel model)
        {
            this.view = view;
            this.model = model;
            this.view.Source = this.model.GetAllTables();
            view.Closing += this.View_Closing;
            view.AddItemAttempt += this.View_AddItemAttempt;
            view.TableSelected += this.View_TableSelected;
        }

        public Type CurrentType { get; protected set; }

        protected override View View => view as View;
        protected override Model Model => model as Model;

        private readonly DbEditView view;
        private readonly DbEditModel model;

        public override event EventHandler<ControllerClosedEventArgs> ControllerClosed;

        public override void Show() => View.ShowDialog();
        public override void Close() => View.Close();

        private void View_AddItemAttempt(object sender, System.Windows.Controls.AddingNewItemEventArgs e)
        {
            var dg = sender as System.Windows.Controls.DataGrid;
            long id;
            var constructor = CurrentType.GetConstructor(new[] { typeof(long) });
            if (constructor != null) // entity has constructor with one parameter => it has 'Id' property
            {
                if (dg.Items.Count > 1)
                {
                    dynamic entityBeforeAdded = dg.Items[dg.Items.Count - 2];
                    id = entityBeforeAdded.Id + 1;
                }
                else
                {
                    id = 1;
                }
                e.NewItem = constructor.Invoke(new object[] { id });
            }
            else
            {
                constructor = e.NewItem.GetType().GetConstructor(Type.EmptyTypes);
                e.NewItem = constructor.Invoke(Array.Empty<object>());
            }
        }

        private void View_TableSelected(object sender, TableSelectedEventArgs e)
        {
            CurrentType = e.Type;
        }

        private void View_Closing(object sender, CancelEventArgs e)
        {
            ControllerClosedEventArgs.CloseReason closeReason;
            switch (DbEditView.ConfirmChanges())
            {
                case true:
                    closeReason = ControllerClosedEventArgs.CloseReason.Success;
                    model.Save();
                    break;
                case false:
                    closeReason = ControllerClosedEventArgs.CloseReason.Abort;
                    break;
                case null:
                    e.Cancel = true;
                    return;
            }

            ControllerClosed.Invoke(this, new ControllerClosedEventArgs(closeReason));
        }
    }
}
