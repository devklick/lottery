import { useMutation, useQuery } from "@tanstack/react-query";
import { useNavigate, useParams } from "react-router-dom";
import gameService from "../gameService";
import { EditGameRequestBody, EditGameResponse } from "./editGame.schema";

import CreateOrEditGame from "../CreateOrEditGame/CreateOrEditGame";

const placeholders: EditGameRequestBody = {
  name: "placeholder",
  closeTime: new Date(),
  drawTime: new Date(),
  maxSelections: 1,
  prizes: [
    { position: 1, numberMatchCount: 5 },
    { position: 2, numberMatchCount: 4 },
    { position: 3, numberMatchCount: 3 },
  ],
  selectionsRequiredForEntry: 5,
  startTime: new Date(),
  state: "enabled",
};

interface EditGameProps {}

interface Params extends Record<string, string | undefined> {
  id: string;
}

function EditGame({}: EditGameProps) {
  const { id } = useParams<Params>();
  const navigate = useNavigate();

  const query = useQuery({
    queryKey: ["game", id],
    queryFn: async () => await gameService.getGame({ route: { id: id! } }),
    refetchInterval: 0,
  });

  const mutation = useMutation<EditGameResponse, unknown, EditGameRequestBody>({
    mutationFn: async (request) =>
      gameService.editGame({ body: request, route: { id: id! } }),
    onSuccess: async (response) => navigate(`/games/${response.id}`),
  });

  const initialValues: EditGameRequestBody = {
    closeTime: query.data?.closeTime ?? placeholders.closeTime,
    drawTime: query.data?.drawTime ?? placeholders.drawTime,
    name: query.data?.name ?? placeholders.name,
    maxSelections: query.data?.selections.length ?? placeholders.maxSelections,
    prizes: query.data?.prizes ?? placeholders.prizes,
    selectionsRequiredForEntry:
      query.data?.selectionsRequiredForEntry ??
      placeholders.selectionsRequiredForEntry,
    startTime: query.data?.startTime ?? placeholders.startTime,
    state: query.data?.state ?? placeholders.state,
  };

  return (
    <CreateOrEditGame
      mode="edit"
      initialValues={initialValues}
      mutationFn={mutation.mutateAsync}
      disabled={(form) =>
        query.data?.gameStatus !== "future" || form.isTouched()
      }
      onCancel={() => navigate(`/games/${id}`)}
    />
  );
}

export default EditGame;
