using System.Linq;

namespace PNC.Models;

/// <summary>
/// Extensions pour la classe Document
/// </summary>
public static class DocumentExtensions
{
    /// <summary>
    /// Vérifie si le document a un statut de succès
    /// </summary>
    /// <param name="document">Le document à vérifier</param>
    /// <returns>True si le statut est un succès</returns>
    public static bool IsSuccess(this Document document)
    {
        return !string.IsNullOrEmpty(document.Status) && 
               DocumentStatus.SuccessStatuses.Contains(document.Status);
    }

    /// <summary>
    /// Vérifie si le document a un statut d'échec
    /// </summary>
    /// <param name="document">Le document à vérifier</param>
    /// <returns>True si le statut est un échec</returns>
    public static bool IsFailure(this Document document)
    {
        return !string.IsNullOrEmpty(document.Status) && 
               DocumentStatus.FailureStatuses.Contains(document.Status);
    }

    /// <summary>
    /// Vérifie si le document est en cours de traitement
    /// </summary>
    /// <param name="document">Le document à vérifier</param>
    /// <returns>True si le document est en cours</returns>
    public static bool IsInProgress(this Document document)
    {
        return !string.IsNullOrEmpty(document.Status) && 
               DocumentStatus.InProgressStatuses.Contains(document.Status);
    }

    /// <summary>
    /// Vérifie si le document est une photo capturée
    /// </summary>
    /// <param name="document">Le document à vérifier</param>
    /// <returns>True si c'est une photo capturée</returns>
    public static bool IsPhotoCaptured(this Document document)
    {
        return document.Status == DocumentStatus.PhotoCapturee;
    }

    /// <summary>
    /// Vérifie si le document est une empreinte capturée
    /// </summary>
    /// <param name="document">Le document à vérifier</param>
    /// <returns>True si c'est une empreinte capturée</returns>
    public static bool IsFingerprintCaptured(this Document document)
    {
        return document.Status == DocumentStatus.EmpreintesCapturees;
    }

    /// <summary>
    /// Vérifie si le document est inséré
    /// </summary>
    /// <param name="document">Le document à vérifier</param>
    /// <returns>True si le document est inséré</returns>
    public static bool IsDocumentInserted(this Document document)
    {
        return document.Status == DocumentStatus.DocumentInsere;
    }

    /// <summary>
    /// Obtient le libellé du statut en français
    /// </summary>
    /// <param name="document">Le document</param>
    /// <returns>Le libellé du statut</returns>
    public static string GetStatusLabel(this Document document)
    {
        return document.Status switch
        {
            DocumentStatus.PhotoCapturee => "Photo Capturée",
            DocumentStatus.DocumentInsere => "Document Inséré",
            DocumentStatus.EmpreintesCapturees => "Empreintes Capturées",
            DocumentStatus.EnCours => "En Cours",
            DocumentStatus.Echec => "Échec",
            DocumentStatus.EnAttente => "En Attente",
            DocumentStatus.Termine => "Terminé",
            DocumentStatus.Archive => "Archivé",
            DocumentStatus.Supprime => "Supprimé",
            _ => "Statut Inconnu"
        };
    }

    /// <summary>
    /// Obtient la classe CSS pour le statut
    /// </summary>
    /// <param name="document">Le document</param>
    /// <returns>La classe CSS</returns>
    public static string GetStatusCssClass(this Document document)
    {
        return document.Status switch
        {
            DocumentStatus.PhotoCapturee => "status-success",
            DocumentStatus.DocumentInsere => "status-success",
            DocumentStatus.EmpreintesCapturees => "status-success",
            DocumentStatus.EnCours => "status-warning",
            DocumentStatus.Echec => "status-danger",
            DocumentStatus.EnAttente => "status-info",
            DocumentStatus.Termine => "status-success",
            DocumentStatus.Archive => "status-secondary",
            DocumentStatus.Supprime => "status-danger",
            _ => "status-secondary"
        };
    }
}
