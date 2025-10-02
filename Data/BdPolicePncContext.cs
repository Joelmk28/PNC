using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PNC.Models;

namespace PNC.Data;

public partial class BdPolicePncContext : DbContext
{
    public BdPolicePncContext()
    {
    }

    public BdPolicePncContext(DbContextOptions<BdPolicePncContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Commissariat> Commissariats { get; set; }

    public virtual DbSet<Conjoint> Conjoints { get; set; }

    public virtual DbSet<DistinctionHonorifique> DistinctionHonorifiques { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Empreinte> Empreintes { get; set; }

    public virtual DbSet<Enfant> Enfants { get; set; }

    public virtual DbSet<Enqueteur> Enqueteurs { get; set; }

    public virtual DbSet<FonctionBefore1997> FonctionBefore1997s { get; set; }

    public virtual DbSet<Formation> Formations { get; set; }

    public virtual DbSet<Fri> Fris { get; set; }

    public virtual DbSet<HistAffectation> HistAffectations { get; set; }

    public virtual DbSet<HistFonction> HistFonctions { get; set; }

    public virtual DbSet<HistGrade> HistGrades { get; set; }

    public virtual DbSet<Langue> Langues { get; set; }

    public virtual DbSet<Nutp> Nutps { get; set; }

    public virtual DbSet<PersonnePrevenir> PersonnePrevenirs { get; set; }

    public virtual DbSet<Policier> Policiers { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<Utilisateur> Utilisateurs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<Audit> Audits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=JMK;Database=bdPolicePNC;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Commissariat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Commissariat_PK");

            entity.ToTable("Commissariat");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.District)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("district");
            entity.Property(e => e.Nom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.Province)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("province");
            entity.Property(e => e.Territoire)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("territoire");
        });

        modelBuilder.Entity<Conjoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Conjoint_PK");

            entity.ToTable("Conjoint");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.DateMariage)
                .HasColumnType("datetime")
                .HasColumnName("dateMariage");
            entity.Property(e => e.IdPolicier)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idPolicier");
            entity.Property(e => e.Matricule)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("matricule");
            entity.Property(e => e.Nationalite)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nationalite");
            entity.Property(e => e.Nom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.PostNom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("postNom");
            entity.Property(e => e.Prenom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("prenom");
            entity.Property(e => e.Profession)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("profession");

            entity.HasOne(d => d.IdPolicierNavigation).WithMany(p => p.Conjoints)
                .HasForeignKey(d => d.IdPolicier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policier_Conjoint");
        });

        modelBuilder.Entity<DistinctionHonorifique>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DistinctionHonorifique_PK");

            entity.ToTable("DistinctionHonorifique");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.DateDecision)
                .HasColumnType("datetime")
                .HasColumnName("dateDecision");
            entity.Property(e => e.IdPolicier)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idPolicier");
            entity.Property(e => e.Motif)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("motif");
            entity.Property(e => e.NumeroDecision)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numeroDecision");

            entity.HasOne(d => d.IdPolicierNavigation).WithMany(p => p.DistinctionHonorifiques)
                .HasForeignKey(d => d.IdPolicier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policier_DistinctionHonorifique");
        });

        modelBuilder.Entity<Empreinte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Empreint__3213E83FA6E4BA03");

            entity.ToTable("Empreinte");

            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.IdPolicier)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idPolicier");
            entity.Property(e => e.TypeDoigt)
                .HasMaxLength(50)
                .HasColumnName("typeDoigt");

            entity.HasOne(d => d.IdPolicierNavigation).WithMany(p => p.Empreintes)
                .HasForeignKey(d => d.IdPolicier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Empreinte__idPol__5812160E");
        });

        modelBuilder.Entity<Enfant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Enfant_PK");

            entity.ToTable("Enfant");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.DateNaissance)
                .HasColumnType("datetime")
                .HasColumnName("dateNaissance");
            entity.Property(e => e.IdPolicier)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idPolicier");
            entity.Property(e => e.Nom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.PaysNaissance)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("paysNaissance");
            entity.Property(e => e.PostNom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("postNom");
            entity.Property(e => e.Prenom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("prenom");
            entity.Property(e => e.VilleNaissance)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("villeNaissance");

            entity.HasOne(d => d.IdPolicierNavigation).WithMany(p => p.Enfants)
                .HasForeignKey(d => d.IdPolicier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policier_Enfant");
        });

        modelBuilder.Entity<Enqueteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Enqueteur_PK");

            entity.ToTable("Enqueteur");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Adresse)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("adresse");
            entity.Property(e => e.CodeIdentification)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("codeIdentification");
            entity.Property(e => e.Nom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.PostNom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("postNom");
            entity.Property(e => e.Prenom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("prenom");
            entity.Property(e => e.Telephone)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("telephone");
        });

        modelBuilder.Entity<FonctionBefore1997>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FonctionBefore1997_PK");

            entity.ToTable("FonctionBefore1997");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.ActeNomination)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("acteNomination");
            entity.Property(e => e.DateEntree)
                .HasColumnType("datetime")
                .HasColumnName("dateEntree");
            entity.Property(e => e.DernierGrade)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dernierGrade");
            entity.Property(e => e.IdCommissariat)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idCommissariat");
            entity.Property(e => e.Lieu)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("lieu");
            entity.Property(e => e.MatriculeOrigine)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("matriculeOrigine");
            entity.Property(e => e.Ministere)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ministere");
            entity.Property(e => e.NatureGrade)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("natureGrade");
            entity.Property(e => e.Service)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("service");

            entity.HasOne(d => d.IdCommissariatNavigation).WithMany(p => p.FonctionBefore1997s)
                .HasForeignKey(d => d.IdCommissariat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Commissariat_FonctionBefore1997");
        });

        modelBuilder.Entity<Formation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Formation_PK");

            entity.ToTable("Formation");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Annee).HasColumnName("annee");
            entity.Property(e => e.Diplome)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("diplome");
            entity.Property(e => e.Duree)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("duree");
            entity.Property(e => e.Ecole)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ecole");
            entity.Property(e => e.IdPolicier)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idPolicier");
            entity.Property(e => e.Nature)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nature");
            entity.Property(e => e.NomDiplome)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nomDiplome");
            entity.Property(e => e.Pays)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pays");
            entity.Property(e => e.TypeFormation)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("typeFormation");
            entity.Property(e => e.Ville)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ville");

            entity.HasOne(d => d.IdPolicierNavigation).WithMany(p => p.Formations)
                .HasForeignKey(d => d.IdPolicier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policier_Formation");
        });

        modelBuilder.Entity<Fri>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FRI_PK");

            entity.ToTable("FRI");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Annee).HasColumnName("annee");
            entity.Property(e => e.DateRemplissage)
                .HasColumnType("datetime")
                .HasColumnName("dateRemplissage");
            entity.Property(e => e.IdCommissariat)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idCommissariat");
            entity.Property(e => e.IdEnqueteur)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idEnqueteur");
            entity.Property(e => e.IdPolicier)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idPolicier");
            entity.Property(e => e.Jour).HasColumnName("jour");
            entity.Property(e => e.Mois)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("mois");
            entity.Property(e => e.Numero)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("numero");

            entity.HasOne(d => d.IdCommissariatNavigation).WithMany(p => p.Fris)
                .HasForeignKey(d => d.IdCommissariat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Commissariat_FRI");

            entity.HasOne(d => d.IdEnqueteurNavigation).WithMany(p => p.Fris)
                .HasForeignKey(d => d.IdEnqueteur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FRI_Enqueteur");

            entity.HasOne(d => d.IdPolicierNavigation).WithMany(p => p.Fris)
                .HasForeignKey(d => d.IdPolicier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policier_FRI");
        });

        modelBuilder.Entity<HistAffectation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("HistAffectation_PK");

            entity.ToTable("HistAffectation");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.ActeDenomination)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("acteDenomination");
            entity.Property(e => e.DateActe)
                .HasColumnType("datetime")
                .HasColumnName("dateActe");
            entity.Property(e => e.Denomination)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("denomination");
            entity.Property(e => e.IdPolicier)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idPolicier");
            entity.Property(e => e.Lieu)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("lieu");

            entity.HasOne(d => d.IdPolicierNavigation).WithMany(p => p.HistAffectations)
                .HasForeignKey(d => d.IdPolicier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policier_HistAffectation");
        });

        modelBuilder.Entity<HistFonction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("HistFonction_PK");

            entity.ToTable("HistFonction");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.DateFin)
                .HasColumnType("datetime")
                .HasColumnName("dateFin");
            entity.Property(e => e.DatePriseFonction)
                .HasColumnType("datetime")
                .HasColumnName("datePriseFonction");
            entity.Property(e => e.IdPolicier)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idPolicier");
            entity.Property(e => e.IntituleFonction)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("intituleFonction");

            entity.HasOne(d => d.IdPolicierNavigation).WithMany(p => p.HistFonctions)
                .HasForeignKey(d => d.IdPolicier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policier_HistFonction");
        });

        modelBuilder.Entity<HistGrade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("HistGrade_PK");

            entity.ToTable("HistGrade");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.ActeNomination)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("acteNomination");
            entity.Property(e => e.DateActe)
                .HasColumnType("datetime")
                .HasColumnName("dateActe");
            entity.Property(e => e.DateNomination)
                .HasColumnType("datetime")
                .HasColumnName("dateNomination");
            entity.Property(e => e.IdPolicier)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idPolicier");
            entity.Property(e => e.Intitule)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("intitule");

            entity.HasOne(d => d.IdPolicierNavigation).WithMany(p => p.HistGrades)
                .HasForeignKey(d => d.IdPolicier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policier_HistGrade");
        });

        modelBuilder.Entity<Langue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Langue_PK");

            entity.ToTable("Langue");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.IdPolicier)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idPolicier");
            entity.Property(e => e.Libelle)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("libelle");
            entity.Property(e => e.NiveauEcriture).HasColumnName("niveauEcriture");
            entity.Property(e => e.NiveauLecture).HasColumnName("niveauLecture");

            entity.HasOne(d => d.IdPolicierNavigation).WithMany(p => p.Langues)
                .HasForeignKey(d => d.IdPolicier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policier_Langue");
        });

        modelBuilder.Entity<Nutp>(entity =>
        {
            entity.ToTable("NUTP");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.NumeroNutp)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numeroNUTP");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");
        });

        modelBuilder.Entity<PersonnePrevenir>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PersonnePrevenir_PK");

            entity.ToTable("PersonnePrevenir");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Commune)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("commune");
            entity.Property(e => e.IdPolicier)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idPolicier");
            entity.Property(e => e.Nom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.NumeroRue)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("numeroRue");
            entity.Property(e => e.PostNom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("postNom");
            entity.Property(e => e.Prenom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("prenom");
            entity.Property(e => e.Rue)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("rue");
            entity.Property(e => e.Telephone)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("telephone");

            entity.HasOne(d => d.IdPolicierNavigation).WithMany(p => p.PersonnePrevenirs)
                .HasForeignKey(d => d.IdPolicier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policier_PersonnePrevenir");
        });

        modelBuilder.Entity<Policier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Policier_PK");

            entity.ToTable("Policier");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.ActeNominationFonction)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("acteNominationFonction");
            entity.Property(e => e.ActeNominationGrade)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("acteNominationGrade");
            entity.Property(e => e.Annee).HasColumnName("annee");
            entity.Property(e => e.BuCielCiatEscDet)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bu_ciel_ciat_esc_det");
            entity.Property(e => e.CategoriePermis)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("categoriePermis");
            entity.Property(e => e.Commune)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("commune");
            entity.Property(e => e.DateActe)
                .HasColumnType("datetime")
                .HasColumnName("dateActe");
            entity.Property(e => e.DateDelivrancePermis)
                .HasColumnType("datetime")
                .HasColumnName("dateDelivrancePermis");
            entity.Property(e => e.DateEntreePolice)
                .HasColumnType("datetime")
                .HasColumnName("dateEntreePolice");
            entity.Property(e => e.DateNaissance)
                .HasColumnType("datetime")
                .HasColumnName("dateNaissance");
            entity.Property(e => e.DateNomination)
                .HasColumnType("datetime")
                .HasColumnName("dateNomination");
            entity.Property(e => e.DatePriseFonction)
                .HasColumnType("datetime")
                .HasColumnName("datePriseFonction");
            entity.Property(e => e.DeptBnDistGpt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dept_bn_dist_gpt");
            entity.Property(e => e.DistrictOrigine)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("districtOrigine");
            entity.Property(e => e.EtatCivil)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("etatCivil");
            entity.Property(e => e.FonctionActuelle)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("fonctionActuelle");
            entity.Property(e => e.GroupeSanguin)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("groupeSanguin");
            entity.Property(e => e.Jour)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("jour");
            entity.Property(e => e.LieuAffectation)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("lieuAffectation");
            entity.Property(e => e.LieuEntreePolice)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("lieuEntreePolice");
            entity.Property(e => e.Matricule)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("matricule");
            entity.Property(e => e.Mois)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("mois");
            entity.Property(e => e.NatureGrade)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("natureGrade");
            entity.Property(e => e.Nom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.NomUnite)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nomUnite");
            entity.Property(e => e.NumeroNutp)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numeroNUTP");
            entity.Property(e => e.NumeroPermis)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("numeroPermis");
            entity.Property(e => e.NumeroRue)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("numeroRue");
            entity.Property(e => e.Observation)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("observation");
            entity.Property(e => e.PaysNaissance)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("paysNaissance");
            entity.Property(e => e.Photo)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("photo");
            entity.Property(e => e.PostNom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("postNom");
            entity.Property(e => e.Prenom)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("prenom");
            entity.Property(e => e.ProvinceOrigine)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("provinceOrigine");
            entity.Property(e => e.Rhesus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("rhesus");
            entity.Property(e => e.Rue)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("rue");
            entity.Property(e => e.SecPiSciatSousDet)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sec_pi_sciat_sousDet");
            entity.Property(e => e.SecteurOrigine)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("secteurOrigine");
            entity.Property(e => e.Sexe)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("sexe");
            entity.Property(e => e.Signature)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("signature");
            entity.Property(e => e.Telephone)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("telephone");
            entity.Property(e => e.TerritoireOrigine)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("territoireOrigine");
            entity.Property(e => e.UniteAffectation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("uniteAffectation");
            entity.Property(e => e.UniteMere)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("uniteMere");
            entity.Property(e => e.VillageNaissance)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("villageNaissance");
            entity.Property(e => e.VillageOrigine)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("villageOrigine");
            entity.Property(e => e.VilleNaissance)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("villeNaissance");
            
            entity.Property(e => e.IdCommissariat)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idCommissariat");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.IdCommissariatNavigation)
                .WithMany()
                .HasForeignKey(d => d.IdCommissariat)
                .HasConstraintName("FK_Policier_Commissariat");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Document_PK");

            entity.ToTable("Document");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.IdPolicier)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idPolicier");
            entity.Property(e => e.UrlDocument)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("urlDocument");
            entity.Property(e => e.Label)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("label");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.DateCreation)
                .HasColumnType("datetime")
                .HasColumnName("dateCreation");
            entity.Property(e => e.DateModification)
                .HasColumnType("datetime")
                .HasColumnName("dateModification");
            entity.Property(e => e.TypeDocument)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("typeDocument");
            entity.Property(e => e.Extension)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("extension");
            entity.Property(e => e.TailleFichier)
                .HasColumnName("tailleFichier");
            entity.Property(e => e.EstActif)
                .HasColumnName("estActif");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.IdPolicierNavigation)
                .WithMany(p => p.Documents)
                .HasForeignKey(d => d.IdPolicier)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Document_Policier");
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Sport_PK");

            entity.ToTable("Sport");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.IdPolicier)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idPolicier");
            entity.Property(e => e.Libelle)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("libelle");

            entity.HasOne(d => d.IdPolicierNavigation).WithMany(p => p.Sports)
                .HasForeignKey(d => d.IdPolicier)
                .HasConstraintName("FK_Policier_Sport");
        });

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Utilisateur_PK");

            entity.ToTable("Utilisateur");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");

            entity.Property(e => e.NomUtilisateur)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nomUtilisateur");

            entity.Property(e => e.MotDePasse)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("motDePasse");

            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");

            entity.Property(e => e.Prenom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("prenom");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");

            entity.Property(e => e.Telephone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telephone");

            entity.Property(e => e.EstActif)
                .HasColumnName("estActif");

            entity.Property(e => e.DateCreation)
                .HasColumnType("datetime")
                .HasColumnName("dateCreation");

            entity.Property(e => e.DerniereConnexion)
                .HasColumnType("datetime")
                .HasColumnName("derniereConnexion");

            entity.Property(e => e.MotDePasseModifie)
                .HasColumnName("motDePasseModifie");

            entity.Property(e => e.IdEnqueteur)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idEnqueteur");

            entity.Property(e => e.IdRole)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idRole");

            entity.HasOne(d => d.IdEnqueteurNavigation)
                .WithMany()
                .HasForeignKey(d => d.IdEnqueteur)
                .HasConstraintName("FK_Utilisateur_Enqueteur");

            entity.HasOne(d => d.IdRoleNavigation)
                .WithMany()
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Utilisateur_Role");

            entity.HasIndex(e => e.NomUtilisateur)
                .IsUnique()
                .HasDatabaseName("IX_Utilisateur_NomUtilisateur");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Role_PK");

            entity.ToTable("Role");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");

            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");

            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("description");

            entity.Property(e => e.EstActif)
                .HasColumnName("estActif");

            entity.Property(e => e.DateCreation)
                .HasColumnType("datetime")
                .HasColumnName("dateCreation");

            entity.HasIndex(e => e.Nom)
                .IsUnique()
                .HasDatabaseName("IX_Role_Nom");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Permission_PK");

            entity.ToTable("Permission");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");

            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");

            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("description");

            entity.Property(e => e.Module)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("module");

            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("action");

            entity.Property(e => e.EstActif)
                .HasColumnName("estActif");

            entity.Property(e => e.DateCreation)
                .HasColumnType("datetime")
                .HasColumnName("dateCreation");

            entity.HasIndex(e => new { e.Module, e.Action })
                .IsUnique()
                .HasDatabaseName("IX_Permission_Module_Action");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => new { e.IdRole, e.IdPermission }).HasName("RolePermission_PK");

            entity.ToTable("RolePermission");

            entity.Property(e => e.IdRole)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idRole");

            entity.Property(e => e.IdPermission)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idPermission");

            entity.Property(e => e.DateAttribution)
                .HasColumnType("datetime")
                .HasColumnName("dateAttribution");

            entity.Property(e => e.AttribuePar)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("attribuePar");

           

            entity.HasOne(d => d.Role)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RolePermission_Role");

            entity.HasOne(d => d.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.IdPermission)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RolePermission_Permission");
        });

        modelBuilder.Entity<Audit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Audit_PK");

            entity.ToTable("Audit");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");

            entity.Property(e => e.IdUtilisateur)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idUtilisateur");

            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("action");

            entity.Property(e => e.Table)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("table");

            entity.Property(e => e.IdEntite)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("idEntite");

            entity.Property(e => e.AnciennesValeurs)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("anciennesValeurs");

            entity.Property(e => e.NouvellesValeurs)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("nouvellesValeurs");

            entity.Property(e => e.DateAction)
                .HasColumnType("datetime")
                .HasColumnName("dateAction");

            entity.Property(e => e.AdresseIP)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("adresseIP");

            entity.Property(e => e.UserAgent)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("userAgent");

            entity.HasOne(d => d.IdUtilisateurNavigation)
                .WithMany()
                .HasForeignKey(d => d.IdUtilisateur)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Audit_Utilisateur");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}


