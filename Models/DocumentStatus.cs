namespace PNC.Models;

/// <summary>
/// Énumération des statuts possibles pour les documents
/// </summary>
public static class DocumentStatus
{
    /// <summary>
    /// Photo capturée avec succès
    /// </summary>
    public const string PhotoCapturee = "Photo_Capturee";

    /// <summary>
    /// Document inséré avec succès
    /// </summary>
    public const string DocumentInsere = "Document_Insere";

    /// <summary>
    /// Empreintes capturées avec succès
    /// </summary>
    public const string EmpreintesCapturees = "Empreintes_Capturees";

    /// <summary>
    /// En cours de traitement
    /// </summary>
    public const string EnCours = "En_Cours";

    /// <summary>
    /// Échec de la capture/insertion
    /// </summary>
    public const string Echec = "Echec";

    /// <summary>
    /// En attente de traitement
    /// </summary>
    public const string EnAttente = "En_Attente";

    /// <summary>
    /// Traitement terminé avec succès
    /// </summary>
    public const string Termine = "Termine";

    /// <summary>
    /// Document archivé
    /// </summary>
    public const string Archive = "Archive";

    /// <summary>
    /// Document supprimé
    /// </summary>
    public const string Supprime = "Supprime";

    /// <summary>
    /// Obtient tous les statuts disponibles
    /// </summary>
    public static readonly string[] AllStatuses = new[]
    {
        PhotoCapturee,
        DocumentInsere,
        EmpreintesCapturees,
        EnCours,
        Echec,
        EnAttente,
        Termine,
        Archive,
        Supprime
    };

    /// <summary>
    /// Obtient les statuts de succès
    /// </summary>
    public static readonly string[] SuccessStatuses = new[]
    {
        PhotoCapturee,
        DocumentInsere,
        EmpreintesCapturees,
        Termine
    };

    /// <summary>
    /// Obtient les statuts d'échec
    /// </summary>
    public static readonly string[] FailureStatuses = new[]
    {
        Echec,
        Supprime
    };

    /// <summary>
    /// Obtient les statuts en cours
    /// </summary>
    public static readonly string[] InProgressStatuses = new[]
    {
        EnCours,
        EnAttente
    };
}
