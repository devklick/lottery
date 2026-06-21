import { CheckIcon } from "@mantine/core";
import { notifications } from "@mantine/notifications";
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
    const id = notifications.show({
      loading: true,
      title: "Submitting entry",
      message: `Your numbers are being saved (${selectionNumbers.join(", ")})`,
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
    notifications.update({
      id,
      loading: false,
      title: "Entry saved",
      message: `Your numbers have been saved (${selectionNumbers.join(", ")})`,
      icon: <CheckIcon />,
      autoClose: 3000,
      allowClose: true,
    });

    setSelectedNumbers([]);
  }

  // TODO: Improve this, remove the annoying overlay on success
  return (
    <PageSection
      title="Pick Your Numbers"
      collapsable
      subheader={`Pick ${selectionsRequired} numbers to submit your entry`}
      className="create-entry-page-section"
    >
      <SelectionPicker
        requiredCount={selectionsRequired}
        selectedNumbers={selectedNumbers}
        selectionNumbers={selections}
        onSubmit={handleSubmitEntry}
        submitStatus={
          mutation.isPending ? "submitting" : success ? "success" : "waiting"
        }
      />
    </PageSection>
  );
}

export default CreateEntry;
