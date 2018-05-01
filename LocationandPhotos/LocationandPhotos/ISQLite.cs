using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationandPhotos
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
