﻿
using Np.NotesService.Application.Dtos;

namespace Np.NotesService.Application.Relations.GetOutgoingRelations;

public sealed record GetOutgoingRelationsResponse(IEnumerable<RelationItem> Relations);

