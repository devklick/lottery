import { useMutation } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";

import { CreateGameRequest, CreateGameResponse } from "./createGame.schema";
import CreateOrEditGame from "../CreateOrEditGame/CreateOrEditGame";
import gameService from "../gameService";

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface CreateGameProps {}

function getDate(now: Date, daysToAdd: number, hoursToAdd = 0) {
  const date = new Date(now);
  date.setUTCDate(now.getDate() + daysToAdd);
  date.setUTCHours(now.getHours() + hoursToAdd);
  date.setUTCMinutes(0);
  date.setUTCSeconds(0);
  date.setUTCMilliseconds(0);
  return date;
}

// eslint-disable-next-line no-empty-pattern
function CreateGame({}: CreateGameProps) {
  const now = new Date();
  const defaultStartTime = getDate(now, 1);
  const defaultDrawTime = getDate(now, 8);
  const defaultCloseTime = getDate(now, 8, -1);

  const initialValues: CreateGameRequest = {
    name: `Lottery Game - ${now.toDateString()}`,
    state: "enabled",
    startTime: defaultStartTime,
    closeTime: defaultCloseTime,
    drawTime: defaultDrawTime,
    maxSelections: 50,
    selectionsRequiredForEntry: 5,
    prizes: [{ position: 1, numberMatchCount: 5 }],
  };

  const navigate = useNavigate();

  const mutation = useMutation<CreateGameResponse, unknown, CreateGameRequest>({
    mutationFn: async (request) => gameService.createGame(request),
    onSuccess: async (response) => navigate(`/games/${response.id}`),
  });

  return (
    <CreateOrEditGame
      mode="create"
      initialValues={initialValues}
      mutationFn={mutation.mutateAsync}
      submitDisabled={(form) => Object.keys(form.errors).length > 0}
      onCancel={() => navigate("/games")}
    />
  );
}

export default CreateGame;
