﻿namespace AspNet.JwtLearning.App_Start
{
    public class MyFileAppender : log4net.Appender.RollingFileAppender
    {
        private bool isFirstTime = true;

        protected override void OpenFile(string fileName, bool append)
        {
            if (isFirstTime)
            {
                isFirstTime = false;
                return;
            }

            base.OpenFile(fileName, append);
        }
    }
}