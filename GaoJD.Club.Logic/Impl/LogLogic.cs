using GaoJD.Club.BusinessEntity;
using GaoJD.Club.Core;
using GaoJD.Club.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Logic.Impl
{
    public class LogLogic : LogicBase<Log>, ILogLogic
    {
        private ILogRepository logRepository;
        public LogLogic(IRepositoryBase<Log> dalBase, ILogRepository logRepository) : base(dalBase)
        {
            this.logRepository = logRepository;
        }
    }
}
