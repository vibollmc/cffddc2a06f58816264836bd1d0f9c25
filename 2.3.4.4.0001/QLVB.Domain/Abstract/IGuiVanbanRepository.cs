using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IGuiVanbanRepository
    {
        IQueryable<GuiVanban> GuiVanbans { get; }

        int Them(GuiVanban vb);

        int UpdateHoibao(int idvanban, int iddonvi, DateTime? dteNgaygui);

        int UpdateTrangthaiGui(int idvanban, int iddonvi, int intloaivanban);
        int UpdateTrangthaiNhan(int idvanban, int iddonvi, int intloaivanban, enumGuiVanban.inttrangthaiphanhoi trangthai, DateTime ngaythuchien);

        int UpdateTrangthaiNhan(int idvanban, string strmatructinh, string strtendonvi, int intloaivanban,
            enumGuiVanban.inttrangthaiphanhoi trangthai, DateTime ngaythuchien, enumGuiVanban.intloaigui intloaigui);
    }
}
