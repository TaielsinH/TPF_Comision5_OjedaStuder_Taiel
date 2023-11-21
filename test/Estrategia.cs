
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DeepSpace
{

	class Estrategia
	{
		
		
		public String Consulta1( ArbolGeneral<Planeta> arbol)
		{
			int camino = Profundidad(arbol);

			return "El camino es de longitud"+camino;
		}
        public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol)
        {
			bool ataque = false;
			
			Movimiento movimiento = null;
			if(arbol.getDatoRaiz().team == 2) //si el planeta es del bot
			{//esto solo sirve para atacar nodos hijos
				foreach(ArbolGeneral<Planeta> hijo in arbol.getHijos()) 
				{
					if (hijo.getDatoRaiz().team != 2)//si el hijo no es bot, es objetivo
					{
						if(ataque = Atacar(arbol.getDatoRaiz(), hijo.getDatoRaiz()))
						{
							return movimiento = new Movimiento(arbol.getDatoRaiz(),hijo.getDatoRaiz());
						}

					}
				}
			}
			if(arbol.getDatoRaiz().team != 2 && movimiento == null)
			{//si no pudo atacar al hijo, ataca al padre
				int posicion = ComprobarHijos(arbol);
				if (posicion >-1)
				{//si encontró un hijo bot, se evalúa la posibilidad de ataque
					if (Atacar(arbol.getHijos()[posicion].getDatoRaiz(), arbol.getDatoRaiz()))
					{//el hijo ataca al padre
						return movimiento = new Movimiento(arbol.getHijos()[posicion].getDatoRaiz(), arbol.getDatoRaiz());
					}
				}
			}
			if (movimiento == null)
			{//modificacion del else, porque sino no ejecuta esta parte del código
				//seguirá buscando un movimiento recursivamente
				foreach(ArbolGeneral<Planeta> hijo in arbol.getHijos())
				{
					movimiento = CalcularMovimiento(hijo);
					if (movimiento != null)
					{
						break;
					}
				}
			}
            return movimiento;
        }
		public int ComprobarHijos(ArbolGeneral<Planeta> arbol)
		{
			
			int posicion = -1;
			for(int i = 0; i<=arbol.getHijos().Count-1;i++)
			{
				if (arbol.getHijos()[i].getDatoRaiz().team==2)
				{
					posicion = i;
					return posicion;
				}
			}
			return posicion;
		}
		
		public bool Atacar(Planeta bot, Planeta objetivo) 
		{ //metodo auxiliar para determinar si el ataque se puede realizar con exito
			if(bot.population/2 > objetivo.population)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
        public int Profundidad(ArbolGeneral<Planeta> arbol) 
		{
			if (arbol.getDatoRaiz().team == 2)
			{
				return 0;
			}
			foreach(ArbolGeneral<Planeta> hijo in arbol.getHijos())
			{
				int profundidadEnHijo = Profundidad(hijo);
				if(profundidadEnHijo>=0)
				{
					return profundidadEnHijo+1;
				}
			}
			return -1;
		}
		public List<String> RecorridoPorNivelesLista(ArbolGeneral<Planeta> arbol)
		{//el metodo solo es utilizado en planetas de bots, devolverá todos los descendientes más el nodo raiz
			List<String> descendientes = new List<String>();
			Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>();
			if(arbol!=null)
			{
				cola.encolar(arbol);
			
				while(!cola.esVacia()) //itera mientras la cola no esté vacía
				{
					int i = 1;
					ArbolGeneral<Planeta> nodoActual = cola.desencolar();
					descendientes.Add(i+")"+nodoActual.getDatoRaiz().population.ToString()+ ", ");
					i++;
					foreach(ArbolGeneral<Planeta> hijo in nodoActual.getHijos())
					{
						cola.encolar(hijo);
					}
				}
			}
			return descendientes;
		}
		

		public String Consulta2( ArbolGeneral<Planeta> arbol)
		{//devuelve un listado con todos los planetas descendientes del planeta bot
			List<String> listado = null;
			if(arbol.getDatoRaiz().EsPlanetaDeLaIA())
			{
                listado = RecorridoPorNivelesLista(arbol);
            }
			else
			{
				Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>();
				cola.encolar(arbol);
				while (!cola.esVacia())//recorre en post orden
				{
					ArbolGeneral<Planeta> nodoActual = cola.desencolar();
					foreach (ArbolGeneral<Planeta> hijo in arbol.getHijos())
					{
						if(hijo.getDatoRaiz().EsPlanetaDeLaIA())
						{
							listado = RecorridoPorNivelesLista(hijo);
                            return string.Join(Environment.NewLine, listado);
                        }
						else if (hijo != null)
						{

							cola.encolar(hijo);
						}
					}
					while(!cola.esVacia())
					{
                        string respuesta = Consulta2(cola.desencolar());
						if(respuesta != "")
						{
							return respuesta;
						}
                    }
					
				}
			}

			if(listado != null)
			{
                return string.Join(Environment.NewLine, listado);
            }
			else
			{
				return "";

            }
			
		}


		public String Consulta3( ArbolGeneral<Planeta> arbol)
		{
			string resultado = "";
            Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>();
            Cola<ArbolGeneral<Planeta>> colaEnOrden = new Cola<ArbolGeneral<Planeta>>();

            cola.encolar(arbol);

            while (!cola.esVacia())//encolo en orden todos los nodos en una cola auxiliar
            {
                ArbolGeneral<Planeta> nodoActual = cola.desencolar();

                foreach (ArbolGeneral<Planeta> hijo in nodoActual.getHijos())
                {
                    cola.encolar(hijo);
                }

                colaEnOrden.encolar(nodoActual);
            }
			
			ArbolGeneral<Planeta> raiz = colaEnOrden.desencolar();
			resultado += $"Nivel 1: Población Total: {raiz.getDatoRaiz().Poblacion()}, Población Promedio: {raiz.getDatoRaiz().Poblacion()}\n";
			int sumatoria = 0;
			for(int i = 1; i<=5;i++) //nivel 2
			{
				ArbolGeneral<Planeta> nodoActual = colaEnOrden.desencolar();
				sumatoria += nodoActual.getDatoRaiz().Poblacion();
			}
			double poblacionPromedio = sumatoria / 5;
			resultado += $"Nivel 2: Poblacion Total: {sumatoria}, Población Promedio: {poblacionPromedio:F2}\n";
			sumatoria = 0; //reseteo la sumatoria
			for(int i = 1; i<=15; i++) //empiezo las operaciones para nivel 3
			{
                ArbolGeneral<Planeta> nodoActual = colaEnOrden.desencolar();
                sumatoria += nodoActual.getDatoRaiz().Poblacion();
            }
			poblacionPromedio = sumatoria / 15;
            resultado += $"Nivel 3: Poblacion Total: {sumatoria}, Población Promedio: {poblacionPromedio:F2}\n";
			sumatoria = 0;
            for (int i = 1; i <= 15; i++) //empiezo las opeaciones para nivel 4
            {
                ArbolGeneral<Planeta> nodoActual = colaEnOrden.desencolar();
                sumatoria += nodoActual.getDatoRaiz().Poblacion();
            }
            poblacionPromedio = sumatoria / 15;
            resultado += $"Nivel 4: Poblacion Total: {sumatoria}, Población Promedio: {poblacionPromedio:F2}\n";
            return resultado;
		}
		
		
	}
}
