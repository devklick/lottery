import { Group, Pagination, Select } from "@mantine/core";

interface PaginationBarProps {
  totalPages?: number;
  totalItems?: number;
  page: number;
  onPageChanged(page: number): void;
  limit: number;
  limits?: number[];
  onLimitChanged(limit: number): void;
}

export default function PaginationBar({
  limit,
  onLimitChanged,
  onPageChanged,
  page,
  totalItems,
  totalPages = Math.max(Math.ceil((totalItems ?? 0) / limit), 1),
  limits,
}: PaginationBarProps) {
  return (
    <Group gap={"lg"} align={"center"} w={"100%"} justify="center">
      <Pagination total={totalPages} value={page} onChange={onPageChanged} />
      <Select
        w={70}
        value={limit}
        defaultValue={limit}
        data={limits}
        onChange={(value) => onLimitChanged(Number(value))}
        allowDeselect={false}
      />
    </Group>
  );
}
