import { useState } from "react";
import SelectionPicker from "./SelectionPicker";
import { Modal } from "@mantine/core";
import { useMutation } from "@tanstack/react-query";
import gameService from "../gameService";
import { useDisclosure } from "@mantine/hooks";

interface EditEntryProps {
  selectionNumbers: ReadonlyArray<number>;
  selectedNumbers: ReadonlyArray<number>;
  onClose(): void;
  entryId: string;
}

function EditEntry({
  selectionNumbers,
  selectedNumbers: _selectedNumbers,
  entryId,
  onClose,
}: EditEntryProps) {
  const [opened, { close }] = useDisclosure(true);

  const mutation = useMutation({
    mutationFn: gameService.editEntry,
    onSuccess: () => null,
  });

  async function handleSubmit(selectionNumbers: Array<number>) {
    await mutation.mutateAsync({
      route: { entryId },
      body: {
        selections: selectionNumbers.map((selectionNumber) => ({
          selectionNumber,
        })),
      },
    });
  }
  function handlePickerDone() {
    close();
    onClose();
  }
  function handleClose() {
    close();
    onClose();
  }
  return (
    <Modal opened={opened} onClose={handleClose}>
      <SelectionPicker
        selectionNumbers={selectionNumbers}
        selectedNumbers={_selectedNumbers}
        requiredCount={_selectedNumbers.length}
        onSubmit={handleSubmit}
        submitStatus={
          mutation.status === "idle"
            ? "waiting"
            : mutation.status === "pending"
            ? "submitting"
            : "success"
        }
        onDone={handlePickerDone}
      />
    </Modal>
  );
}

export default EditEntry;
