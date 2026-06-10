import { Flex, Pagination, Select } from "@mantine/core";

interface PaginationBarProps {
  totalPages: number;
  page: number;
  onPageChanged(page: number): void;
  limit: number;
  onLimitChanged(limit: number): void;
}

export default function PaginationBar({
  limit,
  onLimitChanged,
  onPageChanged,
  page,
  totalPages,
}: PaginationBarProps) {
  return (
    <Flex gap={"lg"} align={"center"} mt={"xl"}>
      <Pagination total={totalPages} value={page} onChange={onPageChanged} />
      <Select
        w={70}
        value={limit}
        defaultValue={limit}
        data={[5, 10, 20]}
        onChange={(value) => onLimitChanged(Number(value))}
        allowDeselect={false}
      />
    </Flex>
  );
}
