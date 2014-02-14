using Mesh.FiniteElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mesh.Division
{
    public class SimpleMeshDivider : IMeshDivider
    {
        /// <summary>
        /// glowny konstruktor
        /// </summary>
        /// <param name="percentCompletedDelta">procent po zakonczeniu ktorego informowac o podziale</param>
        public SimpleMeshDivider(double percentCompletedDelta = 0.1)
        {
            PercentCompletedDelta = percentCompletedDelta;
        }

        /// <summary>
        /// dzieli wszystkiepodane elementy zwraca liste tych 
        /// elementow po podziale
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public List<AbstractElement> DivideMesh(List<AbstractElement> elements)
        {
            // wyjatek, jesli przeslano null jako liste elementow
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }

            // lista na nowe elementy
            List<AbstractElement> newElements = new List<AbstractElement>();

            int elementsCount = elements.Count;
            // licznik podzielonych elementow
            int elementsDivided = 0;

            // stopien ukonczenia - po osiagnieciu zadanego stopnia ukonczenia
            // zostanie odpalony PercentCompleted
            double nextCompletionPercentLevel = PercentCompletedDelta;

            // dla kazdego elementu z podanych wykonaj jego podzial
            foreach (AbstractElement element in elements)
            {
                newElements.AddRange(element.Divide());

                // zwieksz licznik elementow podzielonych
                ++elementsDivided;
            
                double completedPercent = (double)elementsDivided / (double)elementsCount;
                // jesli osiagnieto stopien ukonczenia
                if (completedPercent >= nextCompletionPercentLevel)
                {
                    // wywolaj PercentCompleted
                    RaisePercentCompleted(completedPercent);

                    // i zwieksz go o podana delte
                    nextCompletionPercentLevel += PercentCompletedDelta;
                }
            }

            return newElements;
        }

        public event PercentCompletedHandler PercentCompleted;

        /// <summary>
        /// co jaki procent ukonczonych podzialow elementow informowac
        /// </summary>
        public double PercentCompletedDelta { get; private set; }

        /// <summary>
        /// odpal event procentowego zakonczenia podzialu
        /// </summary>
        /// <param name="percent">ile procent podzialu zostala wykonana</param>
        private void RaisePercentCompleted(double percent)
        {
            if (this.PercentCompleted != null)
            {
                if (percent >= 0.0 && percent <= 1)
                {
                    this.PercentCompleted(percent);
                }
            }
        }
    }
}
