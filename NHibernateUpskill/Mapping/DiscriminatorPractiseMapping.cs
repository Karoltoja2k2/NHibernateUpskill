using FluentNHibernate.Mapping;
using NHibernateUpskill.Domain;

namespace NHibernateUpskill.Mapping
{
    public class FileMapping : ClassMap<File>
    {
        public FileMapping()
        {
            Id(x => x.Id);
            Map(x => x.Path).Not.Nullable().Length(255);
            Map(x => x.Name).Not.Nullable().Length(64);
            Map(x => x.FolderId).Not.Nullable();
        }
    }
    
    public class CommentFileMapping : SubclassMap<CommentFile>
    {
        public CommentFileMapping()
        {
            DiscriminatorValue((int)FileTypeKeys.Comment);
        }
    }
    
    public class ArticleFileMapping : SubclassMap<ArticleFile>
    {
        public ArticleFileMapping()
        {
            DiscriminatorValue((int)FileTypeKeys.Article);
        }
    }
}
