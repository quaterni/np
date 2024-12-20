﻿using Np.NotesService.Application.Abstractions.Mediator;

namespace Np.NotesService.Application.Notes.RemoveNote
{
    public sealed record RemoveNoteCommand(Guid NoteId, string IdentityId) :UserRequest(IdentityId), ICommand;
}
