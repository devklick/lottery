import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";

import gameService from "../gameService";
import SelectionPicker from "./SelectionPicker";
import { notifyInfo, updateNotifySuccess } from "../../../common/notifications";
import { GameStatus } from "../../../common/schemas";
import PageSection from "../../../components/PageSection/PageSection";

interface CreateEntryProps {
  gameId: string;
  selectionNumbers: Array<number>;
  selectionsRequired: number;
  gameStatus: GameStatus;
  maxEntriesReached?: boolean;
}

function CreateEntry({
  selectionNumbers: selections,
  gameId,
  selectionsRequired,
  gameStatus,
  maxEntriesReached,
}: CreateEntryProps) {
  const [success, setSuccess] = useState(false);
  const [selectedNumbers, setSelectedNumbers] = useState<ReadonlyArray<number>>(
    [],
  );
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: gameService.createEntry,
    onSuccess: handleMutationSuccess,
  });

  function handleMutationSuccess() {
    setSuccess(true);
    queryClient.refetchQueries({ exact: false, queryKey: ["entries", gameId] });
  }

  async function handleSubmitEntry(selectionNumbers: Array<number>) {
    const id = notifyInfo({
      title: "Submitting entry",
      message: `Your numbers are being saved (${selectionNumbers.join(", ")})`,
      loading: true,
      autoClose: false,
      allowClose: false,
    });
    await mutation.mutateAsync({
      body: {
        gameId,
        selections: selectionNumbers.map((selectionNumber) => ({
          selectionNumber,
        })),
      },
    });
    // TODO: notify when entry fails
    updateNotifySuccess(id, {
      title: "Entry saved",
      message: `Your numbers have been saved (${selectionNumbers.join(", ")})`,
      loading: false,
    });

    setSelectedNumbers([]);
  }

  const subheader = (() => {
    if (maxEntriesReached)
      return "You have reached the maximum number of entries allowed in this game";
    switch (gameStatus) {
      case "future":
        return "Come back when the game opens to pick your numbers";
      case "open":
        return `pick ${selectionsRequired} numbers to submit your entry`;
      case "closed":
      case "resulted":
        return "Game is now closed";
    }
  })();

  // TODO: Improve this, remove the annoying overlay on success
  return (
    <PageSection
      title="Pick Your Numbers"
      collapsable
      subheader={subheader}
      className="create-entry-page-section"
    >
      <SelectionPicker
        requiredCount={selectionsRequired}
        selectedNumbers={selectedNumbers}
        selectionNumbers={selections}
        onSubmit={handleSubmitEntry}
        disabled={gameStatus !== "open" || maxEntriesReached}
        submitStatus={
          mutation.isPending ? "submitting" : success ? "success" : "waiting"
        }
      />
    </PageSection>
  );
}

export default CreateEntry;
