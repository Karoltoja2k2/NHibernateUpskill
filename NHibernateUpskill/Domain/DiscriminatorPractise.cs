using System;

namespace NHibernateUpskill.Domain
{
    public enum FileTypeKeys
    {
        None = 0,
    
        Comment,
        Article
    }
    
    public abstract class File
    {
        public virtual Guid Id { get; set; }
    
        public virtual string Path { get; set; }
    
        public virtual string Name { get; set; }
    
        public virtual int FolderId { get; set; }
    }
    
    public class CommentFile : File
    {
    }
    
    public class ArticleFile : File
    {
    }
}
