using Common;
using Mesh;
using Mesh.Division;
using Mesh.FiniteElement;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeshManipulation.ViewModel
{
    public class RectangularMeshViewModel : INotifyPropertyChanged
    {
        #region CONSTRUCTORS

        public RectangularMeshViewModel()
        {
            RegisterCommands();
        }

        #endregion

        #region INITIALIZATION

        protected void RegisterCommands()
        {
            CreateMeshCommand = new DelegateCommand(CreateMeshCommand_Execute);
            RemoveMeshCommand = new DelegateCommand(RemoveMeshCommand_Execute);
            PutPointCommand = new DelegateCommand<object>(PutPointCommand_Execute, PutPointCommand_CanExecute);
            DivideMeshCommand = new DelegateCommand(DivideMeshCommand_Execute);
        }

        #endregion

        #region FIELDS

        /// <summary>
        /// czy trwa tworzenie siatki
        /// </summary>
        private bool _duringMeshCreation = false;

        #endregion

        #region PROPERTIES

        ObservableCollection<Point> _initialVertices = new ObservableCollection<Point>();
        /// <summary>
        /// punkt poczatkowy dla siatki
        /// </summary>
        public ObservableCollection<Point> InitialVertices
        {
            get { return _initialVertices; }
            set { _initialVertices = value; RaisePropertyChanged(); }
        }

        private RectangularMesh _mesh = null;
        /// <summary>
        /// siatka elementow skonczonych
        /// </summary>
        public RectangularMesh Mesh
        {
            get { return _mesh; }
            set
            {
                _mesh = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Elements");
                RaisePropertyChanged("Vertices");
            }
        }

        /// <summary>
        /// kolekcja elementow siatki - do bindowania
        /// </summary>
        public ObservableCollection<AbstractElement> Elements
        {
            get
            {
                if (Mesh == null)
                {
                    return new ObservableCollection<AbstractElement>();
                }
                return new ObservableCollection<AbstractElement>(Mesh.Elements);
            }
        }

        /// <summary>
        /// kolekcja wierzcholkow elementow siatki
        /// </summary>
        public ObservableCollection<Point> Vertices
        {
            get
            {
                if (Mesh == null || Mesh.Elements == null)
                    return new ObservableCollection<Point>();

                var vertices = from element in Mesh.Elements
                               from point in element.Points
                               select point;

                return new ObservableCollection<Point>(vertices.Distinct());
            }
        }

        #endregion

        #region COMMANDS

        /// <summary>
        /// komenda rozpoczynajaca proces tworzenia siatki
        /// </summary>
        public ICommand CreateMeshCommand { get; set; }
        private void CreateMeshCommand_Execute()
        {
            InitializeMeshCreation();
        }

        /// <summary>
        /// komenda przerywa proces tworzenia siatki i czysci siatke
        /// </summary>
        public ICommand RemoveMeshCommand { get; set; }
        private void RemoveMeshCommand_Execute()
        {
            EndMeshCreation();
            Mesh = null;
        }

        /// <summary>
        /// wstawienie punktu jako wierzcholka siatki (uzywane do utworzenia glownego prostokata siatki)
        /// </summary>
        public ICommand PutPointCommand { get; set; }
        private void PutPointCommand_Execute(object meshArea)
        {
            System.Windows.Controls.Grid area = meshArea as System.Windows.Controls.Grid;

            System.Windows.Point mousePosition = Mouse.GetPosition(area);

            Point p = new Point(mousePosition.X, mousePosition.Y);

            InitialVertices.Add(p);

            if (InitialVertices.Count >= 2)
            {
                FinalizeMeshCreation(InitialVertices[0], InitialVertices[1]);
            }
        }
        /// <summary>
        /// czy mozna wstawic punkt
        /// </summary>
        /// <returns></returns>
        private bool PutPointCommand_CanExecute(object parameter)
        {
            return _duringMeshCreation;
        }

        /// <summary>
        /// komenda dzielaca siatke
        /// </summary>
        public ICommand DivideMeshCommand { get; set; }
        private void DivideMeshCommand_Execute()
        {
            if (Mesh != null)
            {
                IMeshDivider divider = new SimpleMeshDivider();
                Mesh.Elements = divider.DivideMesh(Mesh.Elements);

                RaisePropertyChanged("Elements");
                RaisePropertyChanged("Vertices");
            }
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// inicjalizacja procesu tworzenia siatki
        /// </summary>
        private void InitializeMeshCreation()
        {
            RemoveMeshCommand_Execute();
            _duringMeshCreation = true;
        }

        private void FinalizeMeshCreation(Point p1, Point p2)
        {
            // zakoncz tworzenie siatki
            EndMeshCreation();

            // skonstruuj siatke prostokatna z tych dwoch punktow
            Mesh = new RectangularMesh(p1, p2);
        }

        /// <summary>
        /// zakonczenie procesu tworzenia siatki
        /// </summary>
        private void EndMeshCreation()
        {
            _duringMeshCreation = false;
            InitialVertices.Clear();
        }

        #endregion
        
        #region INotifyPropertyChanged IMPLEMENTATION

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
