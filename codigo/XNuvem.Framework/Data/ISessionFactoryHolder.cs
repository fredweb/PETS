/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using NHibernate;

namespace XNuvem.Data
{
    public interface ISessionFactoryHolder
    {
        ISessionFactory GetSessionFactory();
    }
}