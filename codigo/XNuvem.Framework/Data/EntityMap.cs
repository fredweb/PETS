/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace XNuvem.Data
{
    public interface IEntityMap
    {
        
    }

    /// <summary>
    /// Fluent NHibernate Map see ClassMap for more information
    /// </summary>
    /// <typeparam name="T">Entity to map</typeparam>
    public class EntityMap<T> : ClassMap<T>, IEntityMap where T : class
    {

    }

    /// <summary>
    /// Fluent NHibernate Map see SubclassMap for more information
    /// </summary>
    /// <typeparam name="T">Entity to map</typeparam>
    public class SubEntity<T> : SubclassMap<T>, IEntityMap where T : class
    {

    }
}
