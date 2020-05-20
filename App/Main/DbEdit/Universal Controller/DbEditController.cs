using SDiC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace App.Main.DbEdit.Interfaces
{
    public class DbEditController : IController
    {
        protected DbEditController(IDbEditView view, IDbEditModel model)
        {
            View = view;
            Model = model;
            View.Source = Model.ReadAll();
            (View as Window).Closing += this.DbEditView_Closing;
            View.AddItemAttempt += this.View_AddItemAttempt;
            View.TableSelected += this.View_TableSelected;
        }

        public Type CurrentType { get; protected set; }

        IView IController.View => View as IView;
        IModel IController.Model => Model as IModel;

        public IDbEditView View { get; }
        public IDbEditModel Model { get; }

        public event EventHandler<ControllerClosedEventArgs> ControllerClosed;

        public void Show() => View.ShowDialog();
        public void Close() => View.Close();

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

        private void View_TableSelected(object sender, SDiC.TableSelectedEventArgs e)
        {
            CurrentType = e.Type;
        }

        private void DbEditView_Closing(object sender, CancelEventArgs e)
        {
            ControllerClosedEventArgs.CloseReason closeReason;
            switch (View.ConfirmChanges())
            {
                case true:
                    closeReason = ControllerClosedEventArgs.CloseReason.Success;
                    Model.Save();
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
