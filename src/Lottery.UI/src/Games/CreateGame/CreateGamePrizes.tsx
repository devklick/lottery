import { Button, Stack, Title } from "@mantine/core";
import CreateGamePrize from "./CreateGamePrize";
import { useEffect, useState } from "react";
import {
  CreateGamePrizeRequest,
  CreateGamePrizesRequest,
} from "./createGame.schema";

interface CreateGamePrizesProps {
  onChange: (prizes: CreateGamePrizesRequest) => void;
}

function CreateGamePrizes({ onChange }: CreateGamePrizesProps) {
  const [prizes, setPrizes] = useState<
    Array<CreateGamePrizeRequest & { prizeNo: number }>
  >([{ prizeNo: 1, position: 1, numberMatchCount: 1 }]);

  function addNewPrize() {
    setPrizes([
      ...prizes,
      { prizeNo: prizes.length + 1, numberMatchCount: 1, position: 1 },
    ]);
  }

  useEffect(() => {
    onChange(prizes.map(({ prizeNo, ...prize }) => prize));
  }, [prizes]);

  function removePrize(prizeNo: number) {
    const index = prizes.findIndex((p) => p.prizeNo === prizeNo);
    if (index >= 0) {
      const newPrizes = [...prizes];
      newPrizes.splice(index, 1);
      newPrizes.forEach((p, i) => (p.prizeNo = i + 1));
      console.log(newPrizes);
      setPrizes(newPrizes);
    }
  }

  function handlePrizeChanged(prizeNo: number, prize: CreateGamePrizeRequest) {
    const index = prizes.findIndex((p) => p.prizeNo === prizeNo);
    if (index < 0) return;
    const newPrizes = [...prizes];
    newPrizes.splice(index, 1, { ...prize, prizeNo });
    setPrizes(newPrizes);
  }

  return (
    <Stack align="center">
      <Title size={"h2"}>Prizes</Title>
      {prizes.map((prize) => (
        <CreateGamePrize
          key={prize.prizeNo}
          prizeNo={prize.prizeNo}
          defaultValues={{ ...prize }}
          canRemovePrize={prizes.length > 1}
          removePrize={removePrize}
          onChange={handlePrizeChanged}
        />
      ))}
      <Button onClick={addNewPrize}>Add new prize</Button>
    </Stack>
  );
}

export default CreateGamePrizes;
