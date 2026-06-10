import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import gameService from "../gameService";
import SelectionPicker from "./SelectionPicker";
import PageSection from "../../components/PageSection/PageSection";

interface CreateEntryProps {
  gameId: string;
  selectionNumbers: Array<number>;
  selectionsRequired: number;
}

function CreateEntry({
  selectionNumbers: selections,
  gameId,
  selectionsRequired,
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
    await mutation.mutateAsync({
      body: {
        gameId,
        selections: selectionNumbers.map((selectionNumber) => ({
          selectionNumber,
        })),
      },
    });
  }

  function handleSelectionPickerDone() {
    setSelectedNumbers([]);
    setSuccess(false);
  }

  // TODO: Improve this, remove the annoying overlay on success
  return (
    <PageSection
      title="Pick Your Numbers"
      subheader={`Pick ${selectionsRequired} numbers to submit your entry`}
    >
      <SelectionPicker
        requiredCount={selectionsRequired}
        selectedNumbers={selectedNumbers}
        selectionNumbers={selections}
        onSubmit={handleSubmitEntry}
        onDone={handleSelectionPickerDone}
        submitStatus={
          mutation.isPending ? "submitting" : success ? "success" : "waiting"
        }
      />
    </PageSection>
  );
}

export default CreateEntry;
